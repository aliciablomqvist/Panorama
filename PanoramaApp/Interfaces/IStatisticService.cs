using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Services;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Interfaces
{
    public interface IStatisticsService
{
    Task<UserStatistics> GetUserStatisticsAsync(string userId);
    Task<GroupStatistics> GetGroupStatisticsAsync(int groupId);
}
}