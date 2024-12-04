using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ViewMovieListsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ViewMovieListsModel> _logger;

    public ViewMovieListsModel(ApplicationDbContext context, ILogger<ViewMovieListsModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<MovieList> MovieLists { get; set; } = new();

    public async Task OnGetAsync()
    {
        _logger.LogInformation("Fetching all MovieLists");

        try
        {
            MovieLists = await _context.MovieLists.ToListAsync();

            if (MovieLists.Count == 0)
            {
                _logger.LogWarning("No MovieLists found in the database");
            }
            else
            {
                _logger.LogInformation("Fetched {Count} MovieLists", MovieLists.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching MovieLists");
        }
    }
}