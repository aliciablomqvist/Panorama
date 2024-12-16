// <copyright file="IWinnerService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for Winning movie.
    /// </summary>
    public interface IWinnerService
    {
        /// <summary>
        /// Gets the winning movie asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<(Movie? WinningMovie, int VoteCount)> GetWinningMovieAsync(int groupId);
    }
}
