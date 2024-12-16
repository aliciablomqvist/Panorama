// <copyright file="IVoteService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Threading.Tasks;

    public interface IVoteService
    {
        Task AddVoteAsync(int groupId, int movieId, string userId);

        Task<int> GetVotesForMovieAsync(int movieId);

        Task<Dictionary<int, int>> GetVoteCountsForGroupAsync(int groupId);
    }
}
