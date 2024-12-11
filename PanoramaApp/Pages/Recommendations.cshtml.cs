using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data; 
using PanoramaApp.Services;
using PanoramaApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace PanoramaApp.Pages{
public class RecommendationsModel : PageModel
{
    private readonly TmdbService _tmdbService;
    private readonly ApplicationDbContext _dbContext;

    public Movie CurrentMovie { get; private set; }
    public List<Movie> Recommendations { get; private set; } = new List<Movie>();

    public RecommendationsModel(TmdbService tmdbService, ApplicationDbContext dbContext)
    {
        _tmdbService = tmdbService;
        _dbContext = dbContext;
    }

public async Task<IActionResult> OnGetAsync(int tmdbId)
{
    if (tmdbId == 0)
    {
        ModelState.AddModelError(string.Empty, "Invalid TMDb ID.");
        return Page();
    }

    CurrentMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.TmdbId == tmdbId);

    if (CurrentMovie == null)
    {
        ModelState.AddModelError(string.Empty, "Movie not found in the database.");
        return Page();
    }

    Recommendations = await _tmdbService.GetRecommendationsAsync(tmdbId);

    if (Recommendations == null || !Recommendations.Any())
    {
        Recommendations = new List<Movie>(); 
    }

    return Page();
}
}
}