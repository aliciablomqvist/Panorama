// <copyright file="Index.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public string WelcomeMessage { get; set; } = "Welcome to PanoramaApp!";

        public string UserName { get; private set; } = "Guest";

        public async Task OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            this.UserName = user?.UserName ?? "Guest";
        }
    }
}
