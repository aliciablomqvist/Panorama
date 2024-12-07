using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PanoramaApp.Pages.MovieLists
{
    public class MyFavoritesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyFavoritesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MovieList MovieList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            MovieList = await _context.MovieLists
                .Include(ml => ml.Movies)
                    .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == userId);

            return Page();
        }
    }
}
