using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace PanoramaApp.Pages.Movies
{
    public class ReviewsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReviewsModel(ApplicationDbContext context)
        {
            _context = context;
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
            Movie = await _context.Movies
                .Include(m => m.MovieListItems)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (Movie == null)
            {
                return NotFound();
            }

            Reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

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

            var review = new Review
            {
                MovieId = movieId,
                UserId = userId,
                Content = ReviewContent,
                Rating = Rating,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving review: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving your review. Please try again later.");
                return Page();
            }

            return RedirectToPage(new { movieId = movieId });
        }
    }
}
