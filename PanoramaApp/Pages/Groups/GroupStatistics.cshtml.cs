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

        /// <summary>
        /// Gets or sets the statistics.
        /// </summary>
        /// <value>
        /// The statistics.
        /// </value>
        public GroupStatisticsDto Statistics { get; set; }

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupStatisticsModel"/> class.
        /// </summary>
        /// <param name="statisticsService">The statistics service.</param>
        public GroupStatisticsModel(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
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
