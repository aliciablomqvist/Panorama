using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PanoramaApp.Models;
using PanoramaApp.Data;

namespace PanoramaApp.Pages.Groups
{
    public class CreateGroupModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public List<int> SelectedMovies { get; set; } = new List<int>();

        [BindProperty]
        public List<int> SelectedUsers { get; set; } = new List<int>();

        public List<Movie> Movies => MockData.Movies;
        public List<User> Users => MockData.Users;

        public IActionResult OnPost()
        {
            var group = new Group
            {
                Id = MockData.Groups.Count + 1,
                Name = Name,
                Movies = MockData.Movies.Where(m => SelectedMovies.Contains(m.Id)).ToList(),
                Members = MockData.Users.Where(u => SelectedUsers.Contains(u.Id)).ToList()
            };

            MockData.Groups.Add(group);
            return RedirectToPage("/Groups/ViewGroups");
        }
    }
}
