// <copyright file="ViewMovieLists.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class ViewMovieListsModel : PageModel
    {
        private readonly IMovieListService movieListService;
        private readonly IGroupService groupService;
        private readonly ILogger<ViewMovieListsModel> logger;

        public ViewMovieListsModel(
            IMovieListService movieListService,
            IGroupService groupService,
            ILogger<ViewMovieListsModel> logger)
        {
            this.movieListService = movieListService;
            this.groupService = groupService;
            this.logger = logger;
        }

        public List<MovieList> MovieLists { get; private set; } = new ();

        public List<Group> Groups { get; private set; } = new ();

        public async Task OnGetAsync()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.logger.LogInformation("Fetching MovieLists and Groups for user {UserId}", userId);

            try
            {
                // Hämta filmlistor
                this.MovieLists = await this.movieListService.GetMovieListsForUserAsync(userId);

                if (this.MovieLists.Count == 0)
                {
                    this.logger.LogWarning("No MovieLists found for user {UserId}", userId);
                }
                else
                {
                    this.logger.LogInformation("Fetched {Count} MovieLists for user {UserId}", this.MovieLists.Count, userId);
                }

                // Hämta grupper
                this.Groups = await this.groupService.GetGroupsForUserAsync(userId);

                if (this.Groups.Count == 0)
                {
                    this.logger.LogWarning("No groups found for user {UserId}", userId);
                }
                else
                {
                    this.logger.LogInformation("Fetched {Count} groups for user {UserId}", this.Groups.Count, userId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while fetching MovieLists or Groups.");
            }
        }
    }
}