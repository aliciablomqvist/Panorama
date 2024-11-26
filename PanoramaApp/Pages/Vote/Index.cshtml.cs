using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PanoramaApp.Models;
using PanoramaApp.Data;

namespace PanoramaApp.Pages.Vote
{
    public class VoteModel : PageModel
    {
        public List<Movie> Movies { get; set; } = MockData.Movies;

        public IActionResult OnPostVote(int id)
        {
            var movie = MockData.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                movie.Votes++;
            }
            return RedirectToPage();
        }
    }
}
