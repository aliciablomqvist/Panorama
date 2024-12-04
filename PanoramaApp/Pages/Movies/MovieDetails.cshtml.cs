using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Movies
{
    public class MovieDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MovieDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; } = default!;
        public MovieList? MovieList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await _context.Movies
                .Include(m => m.Group)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return RedirectToPage("/Error");
            }

MovieList = await _context.MovieLists
    .Include(ml => ml.Movies)
        .ThenInclude(mli => mli.Movie)
    .FirstOrDefaultAsync(ml => ml.Id == id);

            return Page();
        }
    }
}
