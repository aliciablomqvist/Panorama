// <copyright file="Reviews.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Services;
    using PanoramaApp.Interfaces;

namespace PanoramaApp.Pages.Movies
{


    public class ReviewsModel : PageModel
    {
        private readonly IReviewService _reviewService;
        private readonly IMovieService _movieService;

        public ReviewsModel(IReviewService reviewService, IMovieService movieService)
        {
            _reviewService = reviewService;
            _movieService = movieService;
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
            
            Movie = await _movieService.GetMovieByIdAsync(movieId);

            if (Movie == null)
            {
                return NotFound();
            }


            Reviews = await _reviewService.GetReviewsForMovieAsync(movieId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int movieId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {

                await _reviewService.AddReviewAsync(movieId, userId, ReviewContent, Rating);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving your review. Please try again later.");
                return Page();
            }

            return RedirectToPage(new { movieId });
        }
    }
}