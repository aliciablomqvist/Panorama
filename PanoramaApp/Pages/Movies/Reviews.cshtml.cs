// <copyright file="Reviews.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace PanoramaApp.Pages.Movies
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class ReviewsModel : PageModel
    {
        private readonly IReviewService reviewService;
        private readonly IMovieService movieService;

        public ReviewsModel(IReviewService reviewService, IMovieService movieService)
        {
            this.reviewService = reviewService;
            this.movieService = movieService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Review content is required.")]
        [MaxLength(1000, ErrorMessage = "Review content cannot exceed 1000 characters.")]
        public string ReviewContent { get; set; }

        [BindProperty]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public Movie Movie { get; set; }

        public IList<Review> Reviews { get; private set; }

        public async Task<IActionResult> OnGetAsync(int movieId)
        {
            this.Movie = await this.movieService.GetMovieByIdAsync(movieId);

            if (this.Movie == null)
            {
                return this.NotFound();
            }

            this.Reviews = await this.reviewService.GetReviewsForMovieAsync(movieId);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(int movieId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToPage("/Account/Login");
            }

            try
            {
                await this.reviewService.AddReviewAsync(movieId, userId, this.ReviewContent, this.Rating);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while saving your review. Please try again later.");
                return this.Page();
            }

            return this.RedirectToPage(new { movieId });
        }
    }
}