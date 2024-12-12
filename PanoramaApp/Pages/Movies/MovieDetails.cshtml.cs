// <copyright file="MovieDetails.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Models;
using PanoramaApp.Services;
using PanoramaApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Movies
{
    public class MovieDetailsModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IMovieListService _movieListService;
        private readonly IReviewService _reviewService;
        private readonly IUrlHelperService _urlHelperService;

        public MovieDetailsModel(
            IMovieService movieService,
            IMovieListService movieListService,
            IReviewService reviewService,
            IUrlHelperService urlHelperService)
        {
            _movieService = movieService;
            _movieListService = movieListService;
            _reviewService = reviewService;
            _urlHelperService = urlHelperService;
        }

        public Movie Movie { get; set; }
        public IList<Review> Reviews { get; private set; } = new List<Review>();

        [BindProperty]
        public string ReviewContent { get; set; }

        [BindProperty]
        public int ReviewRating { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hämta filmens detaljer
            Movie = await _movieService.GetMovieByIdAsync(id);

            if (Movie == null)
            {
                return NotFound();
            }

            // Hämta recensioner
            Reviews = await _reviewService.GetReviewsForMovieAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAddToFavoritesAsync(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            await _movieListService.AddToListAsync("My Favorites", movieId, userId);
            return RedirectToPage(new { id = movieId });
        }

        public async Task<IActionResult> OnPostMarkAsWatchedAsync(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            await _movieListService.AddToListAsync("Watched", movieId, userId);
            return RedirectToPage(new { id = movieId });
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            await _reviewService.AddReviewAsync(movieId, userId, ReviewContent, ReviewRating);
            return RedirectToPage(new { id = movieId });
        }

        public string ConvertToEmbedUrl(string youtubeUrl)
        {
            return _urlHelperService.ConvertToEmbedUrl(youtubeUrl);
        }
    }
}