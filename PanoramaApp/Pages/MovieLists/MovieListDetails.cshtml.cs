using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PanoramaApp.Pages.MovieLists
{
    public class MovieListDetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MovieListDetailsModel> _logger;

    public MovieListDetailsModel(ApplicationDbContext context, ILogger<MovieListDetailsModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public MovieList MovieList { get; set; } = default!;

public async Task<IActionResult> OnGetAsync(int id)
{
    Console.WriteLine($"OnGetAsync called with ID: {id}");
    
    try
    {
        MovieList = await _context.MovieLists
            .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
            .FirstOrDefaultAsync(ml => ml.Id == id);

        if (MovieList == null)
        {
            Console.WriteLine($"MovieList not found for ID: {id}");
            return RedirectToPage("/Error");
        }

        Console.WriteLine($"Fetched MovieList: {MovieList.Name} with {MovieList.Movies.Count} movies.");
        return Page();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in OnGetAsync: {ex.Message}");
        return RedirectToPage("/Error");
    }
}
}
}
