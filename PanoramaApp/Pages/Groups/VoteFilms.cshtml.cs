// <copyright file="VoteFilms.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class VoteFilmsModel : PageModel
    {
        private readonly IGroupService groupService;
        private readonly IVoteService voteService;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoteFilmsModel"/> class.
        /// </summary>
        /// <param name="groupService">The group service.</param>
        /// <param name="voteService">The vote service.</param>
        /// <param name="userManager">The user manager.</param>
        public VoteFilmsModel(IGroupService groupService, IVoteService voteService, UserManager<IdentityUser> userManager)
        {
            this.groupService = groupService;
            this.voteService = voteService;
            this.userManager = userManager;
        }

        public Group Group { get; private set; }

        public int GroupId { get; private set; }

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            this.Group = await this.groupService.GetGroupByIdAsync(groupId);

            if (this.Group == null)
            {
                return this.RedirectToPage("/Error");
            }

            this.GroupId = groupId;
            return this.Page();
        }

        /// <summary>
        /// Called when [post vote asynchronous].
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostVoteAsync(int groupId, int movieId)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (string.IsNullOrEmpty(userId))
            {
                return this.Challenge();
            }

            await this.voteService.AddVoteAsync(groupId, movieId, userId);

            return this.RedirectToPage(new { groupId });
        }

        /// <summary>
        /// Gets the votes for movie asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        public async Task<int> GetVotesForMovieAsync(int movieId)
        {
            return await this.voteService.GetVotesForMovieAsync(movieId);
        }
    }
}
