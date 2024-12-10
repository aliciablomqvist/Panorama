using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.MovieLists
{
    public class PrioritizeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PrioritizeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int MovieListId { get; set; }

        public List<Movie> Movies { get; set; }

        public async Task<IActionResult> OnGetAsync(int movieListId)
        {
            MovieListId = movieListId;
            Movies = await _context.Movies
                .Include(m => m.MovieLists)
                .Where(m => m.MovieLists.Any(ml => ml.Id == movieListId))
                .OrderByDescending(m => m.Priority)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSavePrioritiesAsync([FromBody] List<MoviePriorityUpdate> updates)
        {
            foreach (var update in updates)
            {
                var movie = await _context.Movies
                    .FirstOrDefaultAsync(m => m.Id == update.Id && m.MovieLists.Any(ml => ml.Id == MovieListId));

                if (movie != null)
                {
                    movie.Priority = update.Priority;
                }
            }

            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public class MoviePriorityUpdate
        {
            public int Id { get; set; }
            public int Priority { get; set; }
        }
    }
}
