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
    using PanoramaApp.Models;

    public class ReviewsModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public ReviewsModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Review content is required.")]
        [MaxLength(1000, ErrorMessage = "Review content cannot exceed 1000 characters.")]
        public string ReviewContent { get; set; }

        [BindProperty]
        [Range(1, 5, ErrorMessage = "Rating must be between number 1 and 5.")]
        public int Rating { get; set; }

        public Movie Movie { get; set; }

        public List<Review> Reviews { get; set; }

        public async Task<IActionResult> OnGetAsync(int movieId)
        {
            this.Movie = await this.context.Movies
                .Include(m => m.MovieListItems)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (this.Movie == null)
            {
                return this.NotFound();
            }

            this.Reviews = await this.context.Reviews
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

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

            var review = new Review
            {
                MovieId = movieId,
                UserId = userId,
                Content = this.ReviewContent,
                Rating = this.Rating,
                CreatedAt = DateTime.UtcNow,
            };

            try
            {
                this.context.Reviews.Add(review);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving review: {ex.Message}");
                this.ModelState.AddModelError(string.Empty, "An error occurred while saving your review. Please try again later.");
                return this.Page();
            }

            return this.RedirectToPage(new { movieId = movieId });
        }
    }
}