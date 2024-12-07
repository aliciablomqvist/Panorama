using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Movies
{
    public class AddMovieModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddMovieModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MovieList MovieList { get; set; }

        [BindProperty]
        public List<int> SelectedMovieIds { get; set; } = new List<int>();

        public List<SelectListItem> MovieOptions { get; set; }

public async Task<IActionResult> OnGetAsync(int listId)
{
    Console.WriteLine($"OnGetAsync called with ID: {listId}");
    
    try
    {
        MovieList = await _context.MovieLists
            .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
            .FirstOrDefaultAsync(ml => ml.Id == listId);

        if (MovieList == null)
        {
            Console.WriteLine($"MovieList not found for ID: {listId}");
            return RedirectToPage("/Error");
        }


        var existingMovieIds = MovieList.Movies.Select(mlm => mlm.MovieId).ToList();

        var movies = await _context.Movies
            .Where(m => !existingMovieIds.Contains(m.Id))
            .ToListAsync();

        MovieOptions = movies.Select(m => new SelectListItem
        {
            Value = m.Id.ToString(),
            Text = m.Title
        }).ToList();

        Console.WriteLine($"Fetched MovieList: {MovieList.Name} with {MovieList.Movies.Count} movies.");
        return Page();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in OnGetAsync: {ex.Message}");
        return RedirectToPage("/Error");
    }
}

public async Task<IActionResult> OnPostAsync(int listId)
{
    MovieList = await _context.MovieLists
        .Include(ml => ml.Movies)
        .FirstOrDefaultAsync(ml => ml.Id == listId);

    if (MovieList == null)
    {
        return NotFound();
    }

    if (SelectedMovieIds != null && SelectedMovieIds.Any())
    {
        foreach (var movieId in SelectedMovieIds)
        {
            var movie = await _context.Movies.FindAsync(movieId);

            var movieListItem = new MovieListItem
            {
                MovieListId = listId,
                MovieList = MovieList,
                MovieId = movieId,
                Movie = movie
            };
            MovieList.Movies.Add(movieListItem);
        }

        await _context.SaveChangesAsync();
    }

    return RedirectToPage("/Movies/MovieListDetails", new { id = listId });
}
    }
}
