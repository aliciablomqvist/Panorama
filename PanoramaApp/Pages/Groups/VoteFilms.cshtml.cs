// <copyright file="VoteFilms.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class VoteFilmsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public int MovieId { get; set; }

        public VoteFilmsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public Group Group { get; set; } = default!;

        public int GroupId { get; set; }

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            this.Group = await this.context.Groups
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (this.Group == null)
            {
                return this.RedirectToPage("/Error");
            }

            this.GroupId = groupId;
            return this.Page();
        }

        public async Task<IActionResult> OnPostVoteAsync(int groupId, int movieId)
        {
            var group = await this.context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                return this.RedirectToPage("/Error");
            }

            var vote = new Vote
            {
                MovieId = movieId,
                GroupId = groupId,
                UserId = this.User.Identity.Name,
            };

            this.context.Votes.Add(vote);
            await this.context.SaveChangesAsync();

            return this.RedirectToPage(new { groupId });
        }

        public async Task<int> GetVotesForMovieAsync(int movieId)
        {
            return await this.context.Votes
                .Where(v => v.MovieId == movieId)
                .CountAsync();
        }
    }
}