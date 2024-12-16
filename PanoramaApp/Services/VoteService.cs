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

        public VoteService(ApplicationDbContext context)
        {
            this.context = context;
        }

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

        public async Task<int> GetVotesForMovieAsync(int movieId)
        {
            return await this.context.Votes
                .Where(v => v.MovieId == movieId)
                .CountAsync();
        }

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
