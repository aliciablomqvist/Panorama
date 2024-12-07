using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Microsoft.EntityFrameworkCore;

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
        public string ReviewContent { get; set; }

        [BindProperty]
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

            var userId = User.Identity.Name;

            var review = new Review
            {
                MovieId = movieId,
                UserId = userId,
                Content = ReviewContent,
                Rating = Rating
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { movieId = movieId });
        }
    }
}
