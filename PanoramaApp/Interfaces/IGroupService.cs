using PanoramaApp.Models;

namespace PanoramaApp.Interfaces
{
    public interface IGroupService
    {
        Task<List<Group>> GetGroupsForUserAsync(string userId);
        Task<List<Group>> GetUserGroupsAsync(string userId);
        Task AddMovieListToGroupsAsync(MovieList movieList, List<int> groupIds);
    }
}
