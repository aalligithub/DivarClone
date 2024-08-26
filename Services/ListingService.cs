using System.Data.Common;
using System.Net;
using DivarClone.Areas.Identity.Data;
using DivarClone.Controllers;
using DivarClone.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace DivarClone.Services { 
    public interface IListingService
    {
        Task<bool> CreateListingAsync(Listing listing);

        Task<bool> ProcessImageAsync(Listing listing, IFormFile? ImageFile);

        List<Listing> FilterResult(string category, object categoryEnum);

        List<Listing> SearchResult(string textToSearch);

        List<Listing> ShowUserListings(string Username);

        void DeleteUserListing(int id);

        List<Listing> GetAllListings();

        public Listing GetSpecificListing(int id);

        Task<bool> UpdateListingAsync(Listing listing);
    }

    public class ListingService : IListingService
    {
        private readonly DivarCloneContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AddListingController> _logger;

        public ListingService(DivarCloneContext context, IWebHostEnvironment webHostEnvironment, ILogger<AddListingController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<bool> CreateListingAsync(Listing listing)
        {
            try { 
                listing.DateTimeOfPosting = DateTime.Now;

                _context.Listings.Add(listing);
                await _context.SaveChangesAsync();

                return true;
            }
            catch {
                return false;
            }
        }

        public List<Listing> GetAllListings()
        {
            var listings = _context.Listings.ToList();

            foreach (var listing in listings)
            {
                if (string.IsNullOrEmpty(listing.ImagePath)
                    || !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                {
                    listing.ImagePath = "/images/No_Image_Available.jpg";
                }
            }
            return listings;
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

        public Listing GetSpecificListing(int id)
        {
            var specificListing = _context.Listings.FirstOrDefault(l => l.Id == id);

            if (specificListing != null)
            {
                return specificListing;
            }
            return null;
        }

        public async Task<bool> UpdateListingAsync(Listing listing)
        {
            var existingListing = await _context.Listings.FindAsync(listing.Id);
            if (existingListing == null)
            {
                _logger.LogWarning($"Listing with ID {listing.Id} not found.");
                return false;
            }

            existingListing.Name = listing.Name;
            existingListing.Description = listing.Description;
            existingListing.Price = listing.Price;
            existingListing.Category = listing.Category;
            existingListing.ImagePath = listing.ImagePath;

            try
            {
                _context.Entry(existingListing).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Listing with ID {listing.Id} updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating listing with ID {listing.Id}");
                return false;
            }
        }



        public void DeleteUserListing(int id)
        {
            var listingToDelete = _context.Listings.FirstOrDefault(l => l.Id == id);
            
            if (listingToDelete != null)
            {
                _context.Listings.Remove(listingToDelete);
                _context.SaveChanges();
            }
        }

        public List<Listing> FilterResult(string category, object categoryEnum)
        {            
            var listings = _context.Listings.Where(l => l.Category == (Category)categoryEnum).ToList();

            foreach (var listing in listings)
            {
                if (string.IsNullOrEmpty(listing.ImagePath) ||
                    !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                {
                    listing.ImagePath = "/images/No_Image_Available.jpg";
                }
            }
            return listings;
        }

        public List<Listing> SearchResult(string textToSearch)
        {
            var listings = _context.Listings.Where(l => l.Name.Contains(textToSearch)).ToList();

            foreach (var listing in listings)
            {
                if (string.IsNullOrEmpty(listing.ImagePath) ||
                    !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                {
                    listing.ImagePath = "/images/No_Image_Available.jpg";
                }
            }
            return listings;
        }

        public List<Listing> ShowUserListings(string Username)
        {
            var listings = _context.Listings.Where(l => l.Poster == Username).ToList();

            foreach (var listing in listings)
            {
                if (string.IsNullOrEmpty(listing.ImagePath) ||
                    !System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, listing.ImagePath.TrimStart('/'))))
                {
                    listing.ImagePath = "/images/No_Image_Available.jpg";
                }
            }

            return listings;
        }
    }
}
