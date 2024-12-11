using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.MovieLists
{
    public class CreateMovieListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateMovieListModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public List<int> SelectedGroupIds { get; set; } = new List<int>();

        public List<Group> UserGroups { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            UserGroups = await _context.Groups
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var movieList = new MovieList
            {
                Name = Name,
                OwnerId = user.Id,
                IsShared = SelectedGroupIds.Count > 0
            };

            _context.MovieLists.Add(movieList);
            await _context.SaveChangesAsync();

            foreach (var groupId in SelectedGroupIds)
            {
                var group = await _context.Groups.FindAsync(groupId);
                if (group != null)
                {
                    group.MovieLists.Add(movieList);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/MovieLists/ViewMovieLists");
        }
    }
}
