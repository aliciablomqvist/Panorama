// <copyright file="GroupStatistics.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using PanoramaApp.DTO;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class GroupStatisticsModel : PageModel
    {
        private readonly IStatisticsService statisticsService;

        public GroupStatisticsDto Statistics { get; set; }

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        public GroupStatisticsModel(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public async Task OnGetAsync()
        {
            if (this.GroupId > 0)
            {
                this.Statistics = await this.statisticsService.GetGroupStatisticsAsync(this.GroupId);
            }
            else
            {
                this.Statistics = new GroupStatisticsDto
                {
                    MostWatchedGenre = "N/A",
                    TotalMoviesWatchedByGroup = 0,
                    MostPopularDecade = "N/A",
                };
            }
        }
    }
}
