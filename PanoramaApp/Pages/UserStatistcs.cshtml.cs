// <copyright file="UserStatistcs.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.DTO;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Services;

    public class UserStatisticsModel : PageModel
    {
        private readonly IStatisticsService statisticsService;
        private readonly UserManager<IdentityUser> userManager;

        public UserStatisticsDto Statistics { get; set; }

        public UserStatisticsModel(IStatisticsService statisticsService, UserManager<IdentityUser> userManager)
        {
            this.statisticsService = statisticsService;
            this.userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user != null)
            {
                this.Statistics = await this.statisticsService.GetUserStatisticsAsync(user.Id);
            }
        }
    }
}