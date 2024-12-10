using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PanoramaApp.Pages.MovieLists
{
    public class MovieListDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MovieListDetailsModel> _logger;

        public MovieListDetailsModel(ApplicationDbContext context, ILogger<MovieListDetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public MovieList MovieList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _logger.LogInformation("Fetching MovieList with ID {Id}", id);

            try
            {
                MovieList = await _context.MovieLists
                    .Include(ml => ml.Movies)
                        .ThenInclude(mli => mli.Movie)
                    .FirstOrDefaultAsync(ml => ml.Id == id);

                if (MovieList == null)
                {
                    _logger.LogWarning("MovieList with ID {Id} not found", id);
                    return RedirectToPage("/Error");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching MovieList with ID {Id}", id);
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostDeleteListAsync(int id)
        {
            _logger.LogInformation("Attempting to delete MovieList with ID {Id}", id);

            try
            {
                var movieList = await _context.MovieLists
                    .Include(ml => ml.Movies)
                    .FirstOrDefaultAsync(ml => ml.Id == id);

                if (movieList == null)
                {
                    _logger.LogWarning("MovieList with ID {Id} not found for deletion", id);
                    return RedirectToPage("/Error");
                }

                _context.MovieLists.Remove(movieList);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted MovieList with ID {Id}", id);
                return RedirectToPage("/MovieLists/ViewMovieLists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting MovieList with ID {Id}", id);
                return RedirectToPage("/Error");
            }
        }
    }
}
