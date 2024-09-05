using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Xml.Linq;
using DivarClone.Controllers;
using DivarClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DivarClone.Services
{
    public interface IListingService
    {
        public List<Listing> GetAllListings();

        Task<bool> ProcessImageAsync(Listing listing, IFormFile? ImageFile);

        List<Listing> FilterResult(object categoryEnum);

        List<Listing> SearchResult(string textToSearch);

        List<Listing> ShowUserListings(string Username);

        Task DeleteUserListing(int id);

        Task<bool> CreateListingAsync(Listing listing);

        //public List<Listing> GetSpecificListing(int id);

        //Task<bool> UpdateListingAsync(Listing listing);
    }

    public class ListingService : IListingService
    {
        public string Constr {  get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AddListingController> _logger;

        public ListingService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, ILogger<AddListingController> logger)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DivarCloneContextConnection");
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;

            con = new SqlConnection(Constr);
        }


        public List<Listing> GetAllListings()
        {
            List<Listing> listingsList = new List<Listing>();
            try
            {
                if (con != null && con.State == ConnectionState.Closed) {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_GetListings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {
                    Listing list = new Listing {
                        Id = rdr.GetInt32("Id"),
                        Name = rdr["Name"].ToString(),
                        Description = rdr["Description"].ToString(),
                        Price = Convert.ToInt32(rdr["Price"]),
                        Poster = rdr["Poster"].ToString(),
                        Category = (Category)Enum.Parse(typeof(Category), rdr["Category"].ToString()),
                        DateTimeOfPosting = Convert.ToDateTime(rdr["DateTimeOfPosting"]),
                        ImagePath = rdr["ImagePath"].ToString(),
                    };

                    listingsList.Add(list);
                }

                return listingsList.ToList();

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creating listing");
                throw;
            }
            finally { con.Close(); }
        }


        public async Task<bool> ProcessImageAsync(Listing listing, IFormFile? ImageFile)
        {
            if (ImageFile == null)
            {
                listing.ImagePath = "/images/" + "No_Image_Available.jpg";
                return true;
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadDir);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    listing.ImagePath = "/images/" + fileName;
                    return true;
                }
                catch
                {
                    return false;
                }
               
            } return false;
        }


        public async Task<bool> CreateListingAsync(Listing listing)
        {
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_CreateListing", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", listing.Name);
                cmd.Parameters.AddWithValue("@Description", listing.Description);
                cmd.Parameters.AddWithValue("@Price", listing.Price);
                cmd.Parameters.AddWithValue("@Poster", listing.Poster);
                cmd.Parameters.AddWithValue("@Category", (int)listing.Category);
                cmd.Parameters.AddWithValue("@DateTimeOfPosting", DateTime.Now);
                cmd.Parameters.AddWithValue("@ImagePath", listing.ImagePath);

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                return false;
            }
            finally { 
                con.Close();
            }
        }

        //public List<Listing> GetSpecificListing(int id)
        //{
        //    id = 2011;
        //    List<Listing> listingsList = new List<Listing>();
        //    try
        //    {
        //        con.Open();
        //        var cmd = new SqlCommand("SP_GetSpecificListing", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@Id", id);
        //        cmd.ExecuteNonQueryAsync();

        //        SqlDataReader rdr = cmd.ExecuteReader();

        //        while (rdr.Read())
        //        {
        //            Listing list = new Listing
        //            {
        //                Id = rdr.GetInt32("Id"),
        //                Name = rdr["Name"].ToString(),
        //                Description = rdr["Description"].ToString(),
        //                Price = Convert.ToInt32(rdr["Price"]),
        //                Poster = rdr["Poster"].ToString(),
        //                Category = (Category)Enum.Parse(typeof(Category), rdr["Category"].ToString()),
        //                DateTimeOfPosting = Convert.ToDateTime(rdr["DateTimeOfPosting"]),
        //                ImagePath = rdr["ImagePath"].ToString(),
        //            };

        //            listingsList.Add(list);
        //        }

        //        return listingsList.ToList();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> UpdateListingAsync(Listing listing)
        //{
        //    var existingListing = await _context.Listings.FindAsync(listing.Id);
        //    if (existingListing == null)
        //    {
        //        _logger.LogWarning($"Listing with ID {listing.Id} not found.");
        //        return false;
        //    }

        //    existingListing.Name = listing.Name;
        //    existingListing.Description = listing.Description;
        //    existingListing.Price = listing.Price;
        //    existingListing.Category = listing.Category;
        //    existingListing.ImagePath = listing.ImagePath;

        //    try
        //    {
        //        _context.Entry(existingListing).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //        _logger.LogInformation($"Listing with ID {listing.Id} updated successfully.");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error updating listing with ID {listing.Id}");
        //        return false;
        //    }
        //}


        public async Task DeleteUserListing(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete listing with Id = {Id}", id);

                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_DeleteUserListing", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting listing with Id = {Id}", id);
            }
            finally
            {
                con.Close();
            }
        }


        public List<Listing> FilterResult(object categoryEnum)
        {
            List<Listing> listingsList = new List<Listing>();
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_FilterListing", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@category_enum", categoryEnum);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Listing list = new Listing
                    {
                        Id = rdr.GetInt32("Id"),
                        Name = rdr["Name"].ToString(),
                        Description = rdr["Description"].ToString(),
                        Price = Convert.ToInt32(rdr["Price"]),
                        Poster = rdr["Poster"].ToString(),
                        Category = (Category)Enum.Parse(typeof(Category), rdr["Category"].ToString()),
                        DateTimeOfPosting = Convert.ToDateTime(rdr["DateTimeOfPosting"]),
                        ImagePath = rdr["ImagePath"].ToString(),
                    };

                    listingsList.Add(list);
                }

                return listingsList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                throw;
            }
            finally { con.Close(); }
        }

        public List<Listing> SearchResult(string textToSearch)
        {
            List<Listing> listingsList = new List<Listing>();
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_SearchListing", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TextToSearch", textToSearch);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Listing list = new Listing
                    {
                        Id = rdr.GetInt32("Id"),
                        Name = rdr["Name"].ToString(),
                        Description = rdr["Description"].ToString(),
                        Price = Convert.ToInt32(rdr["Price"]),
                        Poster = rdr["Poster"].ToString(),
                        Category = (Category)Enum.Parse(typeof(Category), rdr["Category"].ToString()),
                        DateTimeOfPosting = Convert.ToDateTime(rdr["DateTimeOfPosting"]),
                        ImagePath = rdr["ImagePath"].ToString(),
                    };

                    listingsList.Add(list);
                }

                return listingsList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                throw;
            }
            finally { con.Close(); }
        }

        public List<Listing> ShowUserListings(string Username)
        {
            List<Listing> listingsList = new List<Listing>();
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_ShowUserListings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", Username);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Listing list = new Listing
                    {
                        Id = rdr.GetInt32("Id"),
                        Name = rdr["Name"].ToString(),
                        Description = rdr["Description"].ToString(),
                        Price = Convert.ToInt32(rdr["Price"]),
                        Poster = rdr["Poster"].ToString(),
                        Category = (Category)Enum.Parse(typeof(Category), rdr["Category"].ToString()),
                        DateTimeOfPosting = Convert.ToDateTime(rdr["DateTimeOfPosting"]),
                        ImagePath = rdr["ImagePath"].ToString(),
                    };

                    listingsList.Add(list);
                }

                return listingsList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                throw;
            }
            finally { con.Close(); }
        }
    }
}
