using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Models;
using PanoramaApp.Data;

namespace PanoramaApp.Pages.Groups
{
    public class ViewWinnerModel : PageModel
    {
        public Group Group { get; set; } = new Group();
        public Movie? WinningMovie { get; set; }

        public void OnGet(int id)
        {
            Group = MockData.Groups.FirstOrDefault(g => g.Id == id);

            if (Group != null && Group.Movies.Any())
            {
                WinningMovie = Group.Movies.OrderByDescending(m => m.Votes).FirstOrDefault();
            }
        }
    }
}
