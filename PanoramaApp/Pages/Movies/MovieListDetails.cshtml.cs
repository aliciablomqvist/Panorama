using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PanoramaApp.Pages.Movies
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
    _logger.LogInformation($"OnGetAsync called with ID: {id}");
    
    try
    {
        MovieList = await _context.MovieLists
            .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
            .FirstOrDefaultAsync(ml => ml.Id == id);

        if (MovieList == null)
        {
            _logger.LogWarning($"MovieList not found for ID: {id}");
            return RedirectToPage("/Error");
        }

        _logger.LogInformation($"Fetched MovieList: {MovieList.Name} with {MovieList.Movies.Count} movies.");
        return Page();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Exception in OnGetAsync");
        return RedirectToPage("/Error");
    }
}
}
}
