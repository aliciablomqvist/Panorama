
using PanoramaApp.Interfaces;
using PanoramaApp.DTO;
using PanoramaApp.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PanoramaApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PanoramaApp.Pages
{
public class UserStatisticsModel : PageModel
    {
        private readonly IStatisticsService _statisticsService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserStatisticsDto Statistics { get; set; }

        public UserStatisticsModel(IStatisticsService statisticsService, UserManager<IdentityUser> userManager)
        {
            _statisticsService = statisticsService;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Statistics = await _statisticsService.GetUserStatisticsAsync(user.Id);
            }
        }
    }
}