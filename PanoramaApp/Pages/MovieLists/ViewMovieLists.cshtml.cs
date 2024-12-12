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
    using PanoramaApp.Models;
    using PanoramaApp.Interfaces;
    public class ViewMovieListsModel : PageModel
    {
        private readonly IMovieListService _movieListService;
        private readonly IGroupService _groupService;
        private readonly ILogger<ViewMovieListsModel> _logger;

        public ViewMovieListsModel(
            IMovieListService movieListService,
            IGroupService groupService,
            ILogger<ViewMovieListsModel> logger)
        {
            _movieListService = movieListService;
            _groupService = groupService;
            _logger = logger;
        }

        public List<MovieList> MovieLists { get; private set; } = new();
        public List<Group> Groups { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _logger.LogInformation("Fetching MovieLists and Groups for user {UserId}", userId);

            try
            {
                // Hämta filmlistor
                MovieLists = await _movieListService.GetMovieListsForUserAsync(userId);

                if (MovieLists.Count == 0)
                {
                    _logger.LogWarning("No MovieLists found for user {UserId}", userId);
                }
                else
                {
                    _logger.LogInformation("Fetched {Count} MovieLists for user {UserId}", MovieLists.Count, userId);
                }

                // Hämta grupper
                Groups = await _groupService.GetGroupsForUserAsync(userId);

                if (Groups.Count == 0)
                {
                    _logger.LogWarning("No groups found for user {UserId}", userId);
                }
                else
                {
                    _logger.LogInformation("Fetched {Count} groups for user {UserId}", Groups.Count, userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching MovieLists or Groups.");
            }
        }
    }
}