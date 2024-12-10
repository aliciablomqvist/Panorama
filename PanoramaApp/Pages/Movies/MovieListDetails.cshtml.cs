using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Movies
{
    public class MovieListDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MovieListDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MovieList MovieList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            MovieList = await _context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Id == id);

            if (MovieList == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSavePrioritiesAsync([FromBody] List<MoviePriorityUpdate> updates)
        {
            if (updates == null || !updates.Any())
            {
                return BadRequest("Invalid data received.");
            }

            foreach (var update in updates)
            {
                var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == update.Id);
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
