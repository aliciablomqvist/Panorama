using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Movies
{
    public class ShareModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ShareModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MovieList MovieList { get; set; }

        public string ShareableLink { get; set; }

        public async Task<IActionResult> OnGetAsync(int listId)
        {
            MovieList = await _context.MovieLists.FindAsync(listId);

            if (MovieList == null)
            {
                return NotFound();
            }

            // Generera en delbar länk
            ShareableLink = Url.Page("/Movies/MovieListDetails", null, new { id = listId }, Request.Scheme);

            return Page();
        }
    }
}
