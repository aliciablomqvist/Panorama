// <copyright file="MovieDetails.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class MovieDetailsModel : PageModel
    {
        private readonly IMovieService movieService;
        private readonly IMovieListService movieListService;
        private readonly IReviewService reviewService;
        private readonly IUrlHelperService urlHelperService;

        public MovieDetailsModel(
            IMovieService movieService,
            IMovieListService movieListService,
            IReviewService reviewService,
            IUrlHelperService urlHelperService)
        {
            this.movieService = movieService;
            this.movieListService = movieListService;
            this.reviewService = reviewService;
            this.urlHelperService = urlHelperService;
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
            this.Movie = await this.movieService.GetMovieByIdAsync(id);

            if (this.Movie == null)
            {
                return this.NotFound();
            }

            // Hämta recensioner
            this.Reviews = await this.reviewService.GetReviewsForMovieAsync(id);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAddToFavoritesAsync(int movieId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToPage("/Account/Login");
            }

            await this.movieListService.AddToListAsync("My Favorites", movieId, userId);
            return this.RedirectToPage(new { id = movieId });
        }

        public async Task<IActionResult> OnPostMarkAsWatchedAsync(int movieId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToPage("/Account/Login");
            }

            await this.movieListService.AddToListAsync("Watched", movieId, userId);
            return this.RedirectToPage(new { id = movieId });
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int movieId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToPage("/Account/Login");
            }

            await this.reviewService.AddReviewAsync(movieId, userId, this.ReviewContent, this.ReviewRating);
            return this.RedirectToPage(new { id = movieId });
        }

        public string ConvertToEmbedUrl(string youtubeUrl)
        {
            return this.urlHelperService.ConvertToEmbedUrl(youtubeUrl);
        }
    }
}
