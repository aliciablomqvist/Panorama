using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Models;
using PanoramaApp.Data;
using Microsoft.EntityFrameworkCore;


namespace PanoramaApp.Pages.Groups
{
   public class ViewWinnerModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ViewWinnerModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Group Group { get; set; } = new Group();
    public Movie? WinningMovie { get; set; }
    public int WinningMovieVoteCount { get; set; } = 0;

    public async Task OnGetAsync(int id)
    {
        Group = await _context.Groups
            .Include(g => g.Movies)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (Group == null || !Group.Movies.Any())
        {
            return;
        }

        var groupVotes = await _context.Votes.Where(v => v.GroupId == id).ToListAsync();

        WinningMovie = Group.Movies
            .OrderByDescending(m => groupVotes.Count(v => v.MovieId == m.Id))
            .FirstOrDefault();

        if (WinningMovie != null)
        {
            WinningMovieVoteCount = groupVotes.Count(v => v.MovieId == WinningMovie.Id);
        }
    }
}
}