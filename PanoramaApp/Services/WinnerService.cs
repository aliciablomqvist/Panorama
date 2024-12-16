// <copyright file="WinnerService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class WinnerService : IWinnerService
    {
        private readonly IGroupService groupService;
        private readonly IVoteService voteService;

        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinnerService"/> class.
        /// </summary>
        /// <param name="groupService">The group service.</param>
        /// <param name="voteService">The vote service.</param>
        /// <param name="userService">The user service.</param>
        public WinnerService(IGroupService groupService, IVoteService voteService, IUserService userService)
        {
            this.groupService = groupService;
            this.voteService = voteService;
            this.userService = userService;
        }

        /// <summary>
        /// Gets the winning movie asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<(Movie? WinningMovie, int VoteCount)> GetWinningMovieAsync(int groupId)
        {
            var user = await this.userService.GetCurrentUserAsync();
            var userId = user.Id;

            var group = await this.groupService.GetGroupWithMoviesAsync(groupId, userId);

            if (group == null || !group.Movies.Any())
            {
                return (null, 0);
            }

            var voteCounts = await this.voteService.GetVoteCountsForGroupAsync(groupId);

            var winningMovie = group.Movies
                .OrderByDescending(m => voteCounts.ContainsKey(m.Id) ? voteCounts[m.Id] : 0)
                .FirstOrDefault();

            var winningVoteCount = winningMovie != null && voteCounts.ContainsKey(winningMovie.Id)
                ? voteCounts[winningMovie.Id]
                : 0;

            return (winningMovie, winningVoteCount);
        }
    }
}
