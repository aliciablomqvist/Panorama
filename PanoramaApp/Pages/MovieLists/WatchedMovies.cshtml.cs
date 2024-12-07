using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PanoramaApp.Pages.MovieLists
{
    public class WatchedModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WatchedModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Movie> WatchedMovies { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = user.Id;

            var watchedList = await _context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Name == "Watched" && ml.OwnerId == userId);

            WatchedMovies = watchedList?.Movies.Select(mli => mli.Movie).ToList() ?? new List<Movie>();

            return Page();
        }
    }
}
