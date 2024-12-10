using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace PanoramaApp.Pages.Movies
{
public class MovieCalendarModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public MovieCalendarModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Movie> Movies { get; set; } // Alla filmer som kan schemaläggas
    public List<MovieCalendar> ScheduledMovies { get; set; } // Schemalagda filmer

    [BindProperty]
    public int MovieId { get; set; }

    [BindProperty]
    public DateTime ScheduledDate { get; set; }

public async Task<IActionResult> OnGetAsync()
{
    // Hämta alla filmer
    Movies = await _context.Movies.ToListAsync();

    // Logga antalet hämtade filmer
    Console.WriteLine($"Number of movies fetched: {Movies.Count}");

    // Hämta schemalagda filmer
    ScheduledMovies = await _context.MovieCalendars
        .Include(mc => mc.Movie)
        .ToListAsync();

    return Page();
}


    public async Task<IActionResult> OnPostScheduleMovieAsync()
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == MovieId);

        if (movie == null)
        {
            ModelState.AddModelError(string.Empty, "The movie does not exist.");
            return await OnGetAsync(); // Återställ sidan med data
        }

        var calendarEntry = new MovieCalendar
        {
            MovieId = MovieId,
            Date = ScheduledDate
        };

        _context.MovieCalendars.Add(calendarEntry);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
}