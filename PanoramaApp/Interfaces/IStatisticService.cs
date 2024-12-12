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

    public interface IStatisticsService
    {
        Task<UserStatisticsDto> GetUserStatisticsAsync(string userId);

        Task<GroupStatisticsDto> GetGroupStatisticsAsync(int groupId);
    }
}