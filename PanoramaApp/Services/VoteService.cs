// <copyright file="VoteService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class VoteService : IVoteService
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoteService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public VoteService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds the vote asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task AddVoteAsync(int groupId, int movieId, string userId)
        {
            var vote = new Vote
            {
                MovieId = movieId,
                GroupId = groupId,
                UserId = userId,
            };

            this.context.Votes.Add(vote);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the votes for movie asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<int> GetVotesForMovieAsync(int movieId)
        {
            return await this.context.Votes
                .Where(v => v.MovieId == movieId)
                .CountAsync();
        }

        /// <summary>
        /// Gets the vote counts for group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<Dictionary<int, int>> GetVoteCountsForGroupAsync(int groupId)
        {
            var votes = await this.context.Votes
                .Where(v => v.GroupId == groupId)
                .GroupBy(v => v.MovieId)
                .Select(g => new { MovieId = g.Key, VoteCount = g.Count() })
                .ToDictionaryAsync(g => g.MovieId, g => g.VoteCount);

            return votes;
        }
    }
}
