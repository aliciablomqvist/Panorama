using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Services;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.DTO;

namespace PanoramaApp.Interfaces
{
    public interface IStatisticsService
{
        Task<UserStatisticsDto> GetUserStatisticsAsync(string userId);
        Task<GroupStatisticsDto> GetGroupStatisticsAsync(int groupId);
}
}