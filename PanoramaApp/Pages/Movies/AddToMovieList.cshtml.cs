using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;

namespace PanoramaApp.Pages.Movies
{
    public class AddToMovieListModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public AddToMovieListModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<MovieList> MovieLists { get; set; } = new List<MovieList>();
    public List<Movie> Movies { get; set; } = new List<Movie>();

    [BindProperty]
    public int MovieListId { get; set; }

    [BindProperty]
    public List<int> SelectedMovies { get; set; } = new List<int>();

    public async Task OnGetAsync()
    {
        var userId = User.Identity?.Name; 
        MovieLists = await _context.MovieLists
            .Where(ml => ml.OwnerId == userId || ml.IsShared)
            .ToListAsync();
        Movies = await _context.Movies.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        foreach (var movieId in SelectedMovies)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie != null)
            {
                var movieListItem = new MovieListItem
                {
                    MovieListId = MovieListId,
                    MovieId = movie.Id
                };
                _context.MovieListItems.Add(movieListItem);
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("/MovieLists/Details", new { id = MovieListId });
    }
}
}