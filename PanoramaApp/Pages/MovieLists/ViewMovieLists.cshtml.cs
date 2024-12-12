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

    public class ViewMovieListsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<ViewMovieListsModel> logger;

        public ViewMovieListsModel(ApplicationDbContext context, ILogger<ViewMovieListsModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public List<MovieList> MovieLists { get; set; } = new ();

        public List<Group> Groups { get; set; } = new ();

        public async Task OnGetAsync()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.logger.LogInformation("Fetching MovieLists and Groups for user {UserId}", userId);

            try
            {
                // Hämta film-listor som användaren äger eller som är delade med grupper användaren är medlem i
                this.MovieLists = await this.context.MovieLists
                    .Include(ml => ml.Movies) // Hämta filmer i varje lista
                    .Include(ml => ml.SharedWithGroups) // Hämta grupper listan är delad med
                    .Where(ml => ml.OwnerId == userId ||
                                 ml.SharedWithGroups.Any(g => g.Members.Any(m => m.UserId == userId)))
                    .ToListAsync();

                if (this.MovieLists.Count == 0)
                {
                    this.logger.LogWarning("No MovieLists found for user {UserId}", userId);
                }
                else
                {
                    this.logger.LogInformation("Fetched {Count} MovieLists for user {UserId}", this.MovieLists.Count, userId);
                }

                // Hämta grupper där användaren är medlem
                this.Groups = await this.context.Groups
                    .Include(g => g.Members)
                        .ThenInclude(m => m.User)
                    .Include(g => g.Movies)
                    .Where(g => g.Members.Any(m => m.UserId == userId))
                    .ToListAsync();

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