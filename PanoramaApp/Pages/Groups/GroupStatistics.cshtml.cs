using PanoramaApp.Services;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using PanoramaApp.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PanoramaApp.Pages.Groups
{
   public class GroupStatisticsModel : PageModel
{
    private readonly IStatisticsService _statisticsService;

    public GroupStatisticsDto Statistics { get; set; }

    [BindProperty(SupportsGet = true)]
    public int GroupId { get; set; }

    public GroupStatisticsModel(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public async Task OnGetAsync()
    {
        if (GroupId > 0)
        {
            Statistics = await _statisticsService.GetGroupStatisticsAsync(GroupId);
        }
        else
        {
            Statistics = new GroupStatisticsDto
            {
                MostWatchedGenre = "N/A",
                TotalMoviesWatchedByGroup = 0,
                MostPopularDecade = "N/A"
            };
        }
    }
}
}