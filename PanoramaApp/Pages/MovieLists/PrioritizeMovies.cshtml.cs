using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PanoramaApp.Pages.MovieLists
{
    public class PrioritizeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PrioritizeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int MovieListId { get; set; }

        public List<Movie> Movies { get; set; }

        public async Task<IActionResult> OnGetAsync(int movieListId)
        {
            MovieListId = movieListId;
            Movies = await _context.Movies
                .Include(m => m.MovieLists)
                .Where(m => m.MovieLists.Any(ml => ml.Id == movieListId))
                .OrderByDescending(m => m.Priority)
                .ToListAsync();

            return Page();
        }

public async Task<IActionResult> OnPostSavePrioritiesAsync([FromBody] List<MoviePriorityUpdate> updates)
{
    if (updates == null || !updates.Any())
    {
        Console.WriteLine("No updates received or failed to bind JSON.");
        return BadRequest("Invalid payload");
    }

    Console.WriteLine("Updates received:");
    foreach (var update in updates)
    {
        Console.WriteLine($"Id: {update.Id}, Priority: {update.Priority}");
    }

    try
    {
        foreach (var update in updates)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == update.Id && m.MovieLists.Any(ml => ml.Id == MovieListId));

            if (movie != null)
            {
                movie.Priority = update.Priority;
            }
        }

        await _context.SaveChangesAsync();
        return new JsonResult(new { success = true });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
        return StatusCode(500, "An internal server error occurred.");
    }
}


        public class MoviePriorityUpdate
        {
             [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("priority")]
    public int Priority { get; set; }
        }
    }
}
