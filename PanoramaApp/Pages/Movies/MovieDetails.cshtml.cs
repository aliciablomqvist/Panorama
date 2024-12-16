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

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class MovieDetailsModel : PageModel
    {
        private readonly IMovieService movieService;
        private readonly IMovieListService movieListService;
        private readonly IReviewService reviewService;
        private readonly IUrlHelperService urlHelperService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieDetailsModel"/> class.
        /// </summary>
        /// <param name="movieService">The movie service.</param>
        /// <param name="movieListService">The movie list service.</param>
        /// <param name="reviewService">The review service.</param>
        /// <param name="urlHelperService">The URL helper service.</param>
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

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.Movie = await this.movieService.GetMovieByIdAsync(id);

            if (this.Movie == null)
            {
                return this.NotFound();
            }

            this.Reviews = await this.reviewService.GetReviewsForMovieAsync(id);
            return this.Page();
        }

        /// <summary>
        /// Called when [post add to favorites asynchronous].
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Called when [post mark as watched asynchronous].
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Called when [post add review asynchronous].
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Converts to embed URL.
        /// </summary>
        /// <param name="youtubeUrl">The youtube URL.</param>
        /// <returns></returns>
        public string ConvertToEmbedUrl(string youtubeUrl)
        {
            return this.urlHelperService.ConvertToEmbedUrl(youtubeUrl);
        }
    }
}
