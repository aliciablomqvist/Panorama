using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PanoramaApp.Pages.Movies
{
    public class MovieDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MovieDetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }

            return Page();
        }

public async Task<IActionResult> OnPostAddToFavoritesAsync(int movieId)
{
 
    var user = await _userManager.GetUserAsync(User);

    if (user == null)
    {
        return RedirectToPage("/Account/Login");
    }

    var userId = user.Id; 
    var favoritesList = await _context.MovieLists
        .Include(ml => ml.Movies)
        .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == userId);

    if (favoritesList == null)
    {
        favoritesList = new MovieList
        {
            Name = "My Favorites",
            OwnerId = userId
        };

        _context.MovieLists.Add(favoritesList);
        await _context.SaveChangesAsync();
    }

    bool movieExists = favoritesList.Movies.Any(mli => mli.MovieId == movieId);
    if (!movieExists)
    {
        var movieListItem = new MovieListItem
        {
            MovieListId = favoritesList.Id,
            MovieId = movieId
        };
        favoritesList.Movies.Add(movieListItem);
        await _context.SaveChangesAsync();
    }

    return RedirectToPage("/Movies/MovieDetails", new { id = movieId });
}

public async Task<IActionResult> OnPostMarkAsWatchedAsync(int movieId)
{
    var user = await _userManager.GetUserAsync(User);

    if (user == null)
    {
        return RedirectToPage("/Account/Login");
    }

    var userId = user.Id;

    var watchedList = await _context.MovieLists
        .Include(ml => ml.Movies)
        .FirstOrDefaultAsync(ml => ml.Name == "Watched" && ml.OwnerId == userId);

    if (watchedList == null)
    {
        watchedList = new MovieList
        {
            Name = "Watched",
            OwnerId = userId
        };

        _context.MovieLists.Add(watchedList);
        await _context.SaveChangesAsync();
    }

    bool movieExists = watchedList.Movies.Any(mli => mli.MovieId == movieId);
    if (!movieExists)
    {
        var movieListItem = new MovieListItem
        {
            MovieListId = watchedList.Id,
            MovieId = movieId
        };

        watchedList.Movies.Add(movieListItem);
        await _context.SaveChangesAsync();
    }

    return RedirectToPage("/Movies/MovieDetails", new { id = movieId });
}
}
}