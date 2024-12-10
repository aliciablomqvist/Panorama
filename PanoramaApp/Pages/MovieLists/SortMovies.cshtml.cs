using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Services;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc; 

namespace PanoramaApp.Pages.MovieLists
{
    public class SortMoviesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SortMoviesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> Movies { get; set; } = new List<Movie>();
 [BindProperty(SupportsGet = true)]
public int MovieListId { get; set; }

public async Task<IActionResult> OnGetSortAsync(string sortBy)
{
    var movies = await _context.Movies
        .Where(m => m.MovieLists.Any(ml => ml.Id == MovieListId))
        .ToListAsync();

    var sortedMovies = MovieSorter.SortByList(movies, sortBy);

    return new JsonResult(sortedMovies.Select(m => new
    {
        id = m.Id,
        title = m.Title,
        releaseDate = m.ReleaseDate
    }));

    }
}
}