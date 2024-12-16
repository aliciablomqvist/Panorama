using PanoramaApp.Models;
using PanoramaApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IGroupService
    {
        Task<List<Group>> GetGroupsForUserAsync(string userId);
         Task<List<Group>> GetDetailedGroupsForUserAsync(string userId);
        Task<List<Group>> GetUserGroupsAsync(string userId);
        Task AddMovieListToGroupsAsync(MovieList movieList, List<int> groupIds);
        Task<Group> GetGroupByIdAsync(int groupId);

        Task<Group> GetGroupWithMoviesAsync(int groupId, string userId);
        Task<bool> IsUserMemberOfGroupAsync(string userId, int groupId); 
        Task<Group> CreateGroupAsync(string name, string ownerId, List<string> userIds);
    }
}
