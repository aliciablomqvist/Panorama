using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Models;
using PanoramaApp.Data;

namespace PanoramaApp.Pages.Movies
{
    public class ExploreMoviesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ExploreMoviesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public async Task OnGetAsync()
        {
 
            Movies = await _context.Movies
                .OrderByDescending(m => m.ReleaseDate) 
                .ToListAsync();
        }
    }
}

