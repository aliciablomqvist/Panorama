using PanoramaApp.Services;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PanoramaApp.Pages.Groups
{
    public class GroupStatisticsModel : PageModel
    {
        private readonly IStatisticsService _statisticsService;

        public GroupStatisticsModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public GroupStatistics Statistics { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        public async Task OnGetAsync()
        {
            Statistics = await _statisticsService.GetGroupStatisticsAsync(GroupId);
        }
    }
}
