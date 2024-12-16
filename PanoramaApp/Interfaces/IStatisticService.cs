// <copyright file="IStatisticService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.DTO;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    /// <summary>
    /// Interface for statistics, both user and group.
    /// </summary>
    public interface IStatisticsService
    {
        /// <summary>
        /// Gets the user statistics asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<UserStatisticsDto> GetUserStatisticsAsync(string userId);

        /// <summary>
        /// Gets the group statistics asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<GroupStatisticsDto> GetGroupStatisticsAsync(int groupId);
    }
}
