﻿using System.Net;
using DivarClone.Areas.Identity.Data;
using DivarClone.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace DivarClone.Services { 
    public interface IListingService
    {
        void AddListing(string listing);

        List<Listing> FilterResult(string category, object categoryEnum);

        List<Listing> SearchResult(string textToSearch);

        List<Listing> ShowUserListings(string Username);

        void DeleteUserListing(int id);

        List<Listing> GetAllListings();
    }

    public class ListingService : IListingService
    {
        private readonly DivarCloneContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ListingService(DivarCloneContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        public void DeleteUserListing(int id)
        {
            var listingToDelete = _context.Listings.FirstOrDefault(l => l.Id == id);

            if (listingToDelete != null)
            {
                _context.Listings.Remove(listingToDelete);
                _context.SaveChanges();
            }
        }

        public void AddListing(string listing)
        {
            //balls
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
