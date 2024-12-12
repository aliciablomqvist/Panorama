// <copyright file="ViewWinner.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class ViewWinnerModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public ViewWinnerModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Group Group { get; set; } = new Group();

        public Movie? WinningMovie { get; set; }

        public int WinningMovieVoteCount { get; set; } = 0;

        public async Task OnGetAsync(int id)
        {
            this.Group = await this.context.Groups
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (this.Group == null || !this.Group.Movies.Any())
            {
                return;
            }

            var groupVotes = await this.context.Votes.Where(v => v.GroupId == id).ToListAsync();

            this.WinningMovie = this.Group.Movies
                .OrderByDescending(m => groupVotes.Count(v => v.MovieId == m.Id))
                .FirstOrDefault();

            if (this.WinningMovie != null)
            {
                this.WinningMovieVoteCount = groupVotes.Count(v => v.MovieId == this.WinningMovie.Id);
            }
        }
    }
}