using PanoramaApp.Services;
using PanoramaApp.Models;
using PanoramaApp.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PanoramaApp.Pages
{
    public class UserStatisticsModel : PageModel
    {
        private readonly IStatisticsService _statisticsService;

        public UserStatisticsModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public UserStatistics Statistics { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                Statistics = await _statisticsService.GetUserStatisticsAsync(userId);
            }
        }
    }
}
