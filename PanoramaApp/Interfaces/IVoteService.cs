// <copyright file="IVoteService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Voting.
    /// </summary>
    public interface IVoteService
    {
        /// <summary>
        /// Adds the vote asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddVoteAsync(int groupId, int movieId, string userId);

        /// <summary>
        /// Gets the votes for movie asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> GetVotesForMovieAsync(int movieId);

        /// <summary>
        /// Gets the vote counts for group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<Dictionary<int, int>> GetVoteCountsForGroupAsync(int groupId);
    }
}
