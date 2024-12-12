using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace PanoramaApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string WelcomeMessage { get; set; } = "Welcome to PanoramaApp!";
        public string UserName { get; private set; } = "Guest";

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserName = user?.UserName ?? "Guest";
        }
    }
}
