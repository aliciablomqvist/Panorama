using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
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
                .FirstOrDefaultAsync(ml => ml.Id == id);

            if (MovieList == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }
    }
}
