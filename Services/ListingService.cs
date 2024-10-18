using System.Data.SqlClient;
using System.Data;
using DivarClone.Controllers;
using DivarClone.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace DivarClone.Services
{
    public interface IListingService
    {
        public List<Listing> GetAllListings();

        List<Listing> FilterResult(object categoryEnum);

        List<Listing> SearchResult(string textToSearch);

        List<Listing> ShowUserListings(string Username);

        Task DeleteUserListing(int id);

        Task<int?> CreateListingAsync(Listing listing);

        public List<Listing> GetSpecificListing(int id);

        Task<bool> UpdateListingAsync(Listing listing);

        List<Listing> RetrieveListingWithImages(SqlDataReader rdr);

        Task<bool> InsertImagePathIntoDB(int? ListingId, List<string> PathToImageFTP);

        Task<bool> UploadImageToFTP(int? ListingId, IFormFile? ImageFile);

        Task<byte[]> GetImagesFromFTPForListing(string ImagePath);
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

		private string ComputeImageHash(string path)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(path));
				return Convert.ToHexString(hashBytes);
			}
		}

        public async Task<bool> UploadImageToFTP(int? ListingId, IFormFile? ImageFile)
        {
			FtpWebRequest ftpRequest = null;
			string ftpUrl = null;
			List<string> PathToImage = new List<string>();

			try
            {
				if (ImageFile == null || ListingId == null)
				{
					throw new ArgumentNullException("ImageFile or ListingId cannot be null.");
				}

                //string ftpHost = Environment.GetEnvironmentVariable("FTP_HOST");
				string ftpHost = "ftp://127.0.0.1:21";

				string imageExtension = Path.GetExtension(ImageFile.FileName);
				string ftpFolder = "/Images/Listings/";
				string imageName = Guid.NewGuid().ToString();
				ftpUrl = ftpHost + ftpFolder + imageName + imageExtension; // Full path with the image name

				ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

				ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
				ftpRequest.UsePassive = false;
				ftpRequest.EnableSsl = false;
				ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
				ftpRequest.Credentials = new NetworkCredential(
                    "Ali","Ak362178"
                    //Environment.GetEnvironmentVariable("FTP_USERNAME"),
                    //Environment.GetEnvironmentVariable("FTP_PASSWORD")
                );

                System.Diagnostics.Debug.WriteLine(ftpUrl);
			}
            catch (Exception ex) {
                _logger.LogError(ex ," Error connecting to ftp server, unresponsive host or wrong credentials");
                return false;
            }

            try
            {
                using (Stream ftpStream = ftpRequest.GetRequestStream())
                {
                    await ImageFile.CopyToAsync(ftpStream);
                    PathToImage.Add(ftpUrl);
				}
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error uploading to ftp server, file transfer failed");
                return false;
            }

            try
            {
                if (await InsertImagePathIntoDB(ListingId, PathToImage) == true)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex) {
				_logger.LogError(ex, " Error adding image path to images table but ftp was successful");
				return false;
			}
        }

		public async Task<bool> InsertImagePathIntoDB(int? listingId, List<string> PathToImageFTP)
		{            
			if (con != null && con.State == ConnectionState.Closed)
			{
				con.Open();
			}
            try
            {
                var cmd = new SqlCommand("SP_InsertImagePathIntoImages", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (var path in PathToImageFTP) {
					cmd.Parameters.AddWithValue("@ListingId", listingId);
                    cmd.Parameters.AddWithValue("@ImagePath", path);

					string imageHashed = ComputeImageHash(path);
					cmd.Parameters.AddWithValue("@ImageHash", imageHashed);

					await cmd.ExecuteNonQueryAsync();
					cmd.Parameters.Clear();
				}
				return true;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex, "failed to add image path to db");
                return false;
            }
            finally {
                System.Diagnostics.Debug.WriteLine("\n Successfully added image path to db");
			}
		}

		public async Task<byte[]> GetImagesFromFTPForListing(string ImagePath)
		{
            FtpWebRequest ftpRequest = null;
            FtpWebResponse ftpResponse = null;
            byte[] imageBytes = null;

            try
            {
                //string ftpHost = Environment.GetEnvironmentVariable("FTP_HOST");
                string ftpHost = "ftp://127.0.0.1:21";

                ftpRequest = (FtpWebRequest)WebRequest.Create(ImagePath);
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                ftpRequest.Credentials = new NetworkCredential(
                    "Ali", "Ak362178"
                //Environment.GetEnvironmentVariable("FTP_USERNAME"),
                //Environment.GetEnvironmentVariable("FTP_PASSWORD")
                );

                ftpRequest.EnableSsl = false;
                ftpRequest.UsePassive = false;
                ftpRequest.UseBinary = false;

                using (ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync())
                using (Stream responseStream = ftpResponse.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Copy the FTP response stream to memory stream
                    await responseStream.CopyToAsync(memoryStream);

                    // Convert memory stream to byte array
                    imageBytes = memoryStream.ToArray();
                }

                _logger.LogTrace($"Download Complete, status: {ftpResponse.StatusDescription}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Failed to convert Image into byte array ");
                ftpResponse?.Close();
                return Array.Empty<byte>();
            }
            finally
            {
                ftpResponse?.Close();
            }

            return imageBytes;
		}

        public async Task<string> DownloadImageAsBase64(string imagePath)
        {
            byte[] imageBytes = await GetImagesFromFTPForListing(imagePath);

            if (imageBytes != null)
            {
                // Convert byte array to Base64 string
                string base64Image = Convert.ToBase64String(imageBytes);
                return $"data:image/jpeg;base64,{base64Image}"; // Assuming the image is JPEG
            }
            return null;
        }

        public List<Listing> RetrieveListingWithImages(SqlDataReader rdr)
		{
			var listingsDictionary = new Dictionary<int, Listing>();

			while (rdr.Read())
			{
				int listingId = rdr.GetInt32("Id");

				// Check if the listing is already added to the dictionary
				if (!listingsDictionary.TryGetValue(listingId, out Listing listing))
				{
					listing = new Listing
					{
						Id = listingId,
						Name = rdr["Name"].ToString(),
						Description = rdr["Description"].ToString(),
						Price = Convert.ToInt32(rdr["Price"]),
						Poster = rdr["Poster"].ToString(),
						Category = (Category)Enum.Parse(typeof(Category), rdr["Category"].ToString()),
						DateTimeOfPosting = Convert.ToDateTime(rdr["DateTimeOfPosting"]),
						ImagePath = new List<string>()
					};
					listingsDictionary[listingId] = listing;
				}

                // Handle image data if available
                if (!rdr.IsDBNull(rdr.GetOrdinal("ImagePath")))
                {
                    //System.Diagnostics.Debug.WriteLine("\n\n A new ImagePath was added for : " + listing.Description);
                    string imagePath = (rdr["ImagePath"].ToString());
                    listing.ImagePath.Add(imagePath);
                } 
                else {
                    //Environment.GetEnvironmentVariable("PATH_TO_DEFAULT_IMAGE")
                    listing.ImagePath.Add("ftp://127.0.0.1/Images/Listings/No_Image_Available.jpg");
                }
                //foreach (string imagePath in listing.ImagePath)
                //{
                //    System.Diagnostics.Debug.WriteLine("\n\n The listing path so far : " + listing.Description + "  " + imagePath);
                //}
                foreach (string imagePath in listing.ImagePath)
                {
                    System.Diagnostics.Debug.WriteLine(listing.ImagePath.Count);
                    System.Diagnostics.Debug.WriteLine("ImagePath: " + imagePath);
                    System.Diagnostics.Debug.WriteLine("Description : " + listing.Description);

                    try
                    {
                        // Await the async method properly to avoid blocking and concurrency issues
                        var base64Image = DownloadImageAsBase64(imagePath).Result;

                        // Only add if the image was successfully downloaded
                        if (!string.IsNullOrEmpty(base64Image))
                        {
                            listing.ImageData.Add(base64Image);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, " Failed to turn Image " + imagePath + " into bytes");
                    }
                }
            }
            return listingsDictionary.Values.ToList();
        }

		public List<Listing> GetAllListings()
        {
			List<Listing> listingsList = new List<Listing>();

			if (con != null && con.State == ConnectionState.Closed) {
                con.Open();
            }

            try
            {
                var cmd = new SqlCommand("SP_GetListingsWithImages", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader rdr = cmd.ExecuteReader();

				listingsList = RetrieveListingWithImages(rdr);
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error Getting Listing list from Listings table");
                throw;
            }
            finally
            {
                con.Close();
            }

            return listingsList;
        }

        public async Task<int?> CreateListingAsync(Listing listing)
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

				int newListingId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                System.Diagnostics.Debug.WriteLine("New Listing Created with Listing Id : "+ newListingId);
				return newListingId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                return null;
            }
            finally { 
                con.Close();
            }
        }

        public List<Listing> GetSpecificListing(int id)
        {
			List<Listing> listing = null;
			try
            {
                con.Open();
                var cmd = new SqlCommand("SP_GetSpecificListing", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader rdr = cmd.ExecuteReader();

                listing = RetrieveListingWithImages(rdr);

                //if (rdr.Read())
                //{
                //	listing = new Listing
                //	{
                //		Id = rdr.GetInt32("Id"),
                //		Name = rdr["Name"].ToString(),
                //		Description = rdr["Description"].ToString(),
                //		Price = Convert.ToInt32(rdr["Price"]),
                //		Poster = rdr["Poster"].ToString(),
                //		Category = (Category)Enum.Parse(typeof(Category), rdr["Category"].ToString()),
                //		DateTimeOfPosting = Convert.ToDateTime(rdr["DateTimeOfPosting"]),
                //		//ImagePath = rdr["ImagePath"].ToString(),
                //	};
                //}

                return listing;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateListingAsync(Listing listing)
        {
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_UpdateListing", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", listing.Id);
                cmd.Parameters.AddWithValue("@Name", listing.Name);
                cmd.Parameters.AddWithValue("@Description", listing.Description);
                cmd.Parameters.AddWithValue("@Price", listing.Price);
                cmd.Parameters.AddWithValue("@Poster", listing.Poster);
                cmd.Parameters.AddWithValue("@Category", (int)listing.Category);
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                //cmd.Parameters.AddWithValue("@ImagePath", listing.ImagePath);

                await cmd.ExecuteNonQueryAsync();

                _logger.LogInformation($"Listing with ID {listing.Id} updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_AddLogToDb", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Operation", "LISTING UPDATE");
                cmd.Parameters.AddWithValue("@Details", "Listing updated FAILED with details : " + listing.Id + " " + listing.Name + " " + listing.Description + " " + listing.Price + " " + listing.Poster + " " + (int)listing.Category + " " + DateTime.Now + " ");
                cmd.Parameters.AddWithValue("@LogDate", DateTime.Now);

                await cmd.ExecuteNonQueryAsync();

                _logger.LogError(ex, $"Error updating listing with ID {listing.Id}");
                return false;
            }
        }
 
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

				var listingsList = RetrieveListingWithImages(rdr);

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

				var listingsList = RetrieveListingWithImages(rdr);

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

				var listingsList = RetrieveListingWithImages(rdr);

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
