using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AddMoviesModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public AddMoviesModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Group Group { get; set; } = default!;
    public List<Movie> AvailableMovies { get; set; } = new List<Movie>();

    [BindProperty]
    public int GroupId { get; set; }

    [BindProperty]
    public List<int> SelectedMovies { get; set; } = new List<int>();

    public async Task OnGetAsync(int groupId)
    {
        Group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

        if (Group != null)
        {
            var assignedMovieIds = await _context.Movies
                .Where(m => m.GroupId == groupId)
                .Select(m => m.Id)
                .ToListAsync();

            AvailableMovies = await _context.Movies
                .Where(m => !assignedMovieIds.Contains(m.Id))
                .ToListAsync();
        }
    }

    public async Task<IActionResult> OnPostAddMoviesAsync()
    {
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == GroupId);
        if (group == null)
        {
            return RedirectToPage("/Error");
        }

        foreach (var movieId in SelectedMovies)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie != null)
            {
                movie.GroupId = group.Id;
                _context.Movies.Update(movie);
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToPage("/Groups/ViewGroups");
    }
}
