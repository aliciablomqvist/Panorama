using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Microsoft.AspNetCore.Identity;

namespace PanoramaApp.Pages.Groups
{
    public class VoteFilmsModel : PageModel
    {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public int MovieId { get; set; }

    public VoteFilmsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


        public Group Group { get; set; } = default!;
        public int GroupId { get; set; }

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            Group = await _context.Groups
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (Group == null)
            {
                return RedirectToPage("/Error");
            }

            GroupId = groupId;
            return Page();
        }

        public async Task<IActionResult> OnPostVoteAsync(int groupId, int movieId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                return RedirectToPage("/Error");
            }

            var vote = new Vote
            {
                MovieId = movieId,
                GroupId = groupId,
                UserId = User.Identity.Name
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { groupId });
        }

        public async Task<int> GetVotesForMovieAsync(int movieId)
        {
            return await _context.Votes
                .Where(v => v.MovieId == movieId)
                .CountAsync();
        }
    }
}
