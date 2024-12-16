// <copyright file="IWinnerService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Threading.Tasks;
    using PanoramaApp.Models;

    public interface IWinnerService
    {
        Task<(Movie? WinningMovie, int VoteCount)> GetWinningMovieAsync(int groupId);
    }
}