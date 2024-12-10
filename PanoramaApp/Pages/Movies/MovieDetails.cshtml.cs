using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace PanoramaApp.Pages.Movies
{
    public class MovieDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ReviewService _reviewService;

        public MovieDetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, ReviewService reviewService)
        {
            _context = context;
            _userManager = userManager;
            _reviewService = reviewService;
        }

        public Movie Movie { get; set; }
        public IList<Review> Reviews { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }

            Reviews = await _reviewService.GetReviewsForMovieAsync(id);
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

            if (!favoritesList.Movies.Any(mli => mli.MovieId == movieId))
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

            if (!watchedList.Movies.Any(mli => mli.MovieId == movieId))
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

        public async Task<IActionResult> OnPostAddReviewAsync(int movieId, string content, int rating)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            await _reviewService.AddReviewAsync(movieId, userId, content, rating);
            return RedirectToPage("/Movies/MovieDetails", new { id = movieId });
        }

public string ConvertToEmbedUrl(string youtubeUrl)
{
    if (string.IsNullOrEmpty(youtubeUrl)) return string.Empty;

    try
    {
        var uri = new Uri(youtubeUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var videoId = query["v"]; // Extrahera 'v'-parametern från URL:en

        return videoId != null ? $"https://www.youtube.com/embed/{videoId}" : string.Empty;
    }
    catch
    {
        // Om något går fel, returnera en tom sträng
        return string.Empty;
    }
}


    }
}