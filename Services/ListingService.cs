using System.Data.SqlClient;
using DivarClone.Controllers;
using DivarClone.Models;
using Microsoft.EntityFrameworkCore;

namespace DivarClone.Services
{
    public interface IListingService
    {

        public List<Listing> GetAllListings();
        Task<bool> ProcessImageAsync(Listing listing, IFormFile? ImageFile);

        //List<Listing> FilterResult(string category, object categoryEnum);
        //List<Listing> SearchResult(string textToSearch);
        //List<Listing> ShowUserListings(string Username);
        //void DeleteUserListing(int id);
        //Task<bool> CreateListingAsync(Listing listing);
        //public Listing GetSpecificListing(int id);
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
                con.Open();
                var cmd = new SqlCommand("SP_GetListings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {
                    Listing list = new Listing {
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
            catch (Exception) {
                throw;
            }

        }


        //public List<Listing> GetAllListings()
        //{
        //    var listings = _context.Listings.ToList();

        //    foreach (var listing in listings)
        //    {
        //        if (string.IsNullOrEmpty(listing.ImagePath)
        //            || !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
        //        {
        //            listing.ImagePath = "/images/No_Image_Available.jpg";
        //        }
        //    }
        //    return listings;
        //}

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


        //public async Task<bool> CreateListingAsync(Listing listing)
        //{
        //    try { 
        //        listing.DateTimeOfPosting = DateTime.Now;

        //        _context.Listings.Add(listing);
        //        await _context.SaveChangesAsync();

        //        return true;
        //    }
        //    catch {
        //        return false;
        //    }
        //}

        //public Listing GetSpecificListing(int id)
        //{
        //    var specificListing = _context.Listings.FirstOrDefault(l => l.Id == id);

        //    if (specificListing != null)
        //    {
        //        return specificListing;
        //    }
        //    return null;
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



        //public void DeleteUserListing(int id)
        //{
        //    var listingToDelete = _context.Listings.FirstOrDefault(l => l.Id == id);
            
        //    if (listingToDelete != null)
        //    {
        //        _context.Listings.Remove(listingToDelete);
        //        _context.SaveChanges();
        //    }
        //}

        //public List<Listing> FilterResult(string category, object categoryEnum)
        //{            
        //    var listings = _context.Listings.Where(l => l.Category == (Category)categoryEnum).ToList();

        //    foreach (var listing in listings)
        //    {
        //        if (string.IsNullOrEmpty(listing.ImagePath) ||
        //            !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
        //        {
        //            listing.ImagePath = "/images/No_Image_Available.jpg";
        //        }
        //    }
        //    return listings;
        //}

        //public List<Listing> SearchResult(string textToSearch)
        //{
        //    var listings = _context.Listings.Where(l => l.Name.Contains(textToSearch)).ToList();

        //    foreach (var listing in listings)
        //    {
        //        if (string.IsNullOrEmpty(listing.ImagePath) ||
        //            !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
        //        {
        //            listing.ImagePath = "/images/No_Image_Available.jpg";
        //        }
        //    }
        //    return listings;
        //}

        //public List<Listing> ShowUserListings(string Username)
        //{
        //    var listings = _context.Listings.Where(l => l.Poster == Username).ToList();

        //    foreach (var listing in listings)
        //    {
        //        if (string.IsNullOrEmpty(listing.ImagePath) ||
        //            !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
        //        {
        //            listing.ImagePath = "/images/No_Image_Available.jpg";
        //        }
        //    }

        //    return listings;
        //}
    }
}
