// <copyright file="IStatisticService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PanoramaApp.DTO;

namespace PanoramaApp.Interfaces
{
    public interface IStatisticsService
    {
        Task<UserStatisticsDto> GetUserStatisticsAsync(string userId);

        Task<GroupStatisticsDto> GetGroupStatisticsAsync(int groupId);
    }
}