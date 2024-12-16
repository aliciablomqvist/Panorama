// <copyright file="IGroupService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public interface IGroupService
    {
        Task<List<Group>> GetGroupsForUserAsync(string userId);

        Task<List<Group>> GetDetailedGroupsForUserAsync(string userId);

        Task<List<Group>> GetUserGroupsAsync(string userId);

        Task<Group?> GetSpecificGroupByIdAsync(int groupId);

        Task AddMovieListToGroupsAsync(MovieList movieList, List<int> groupIds);

        Task<Group> GetGroupByIdAsync(int groupId);

        Task<Group> GetGroupWithMoviesAsync(int groupId, string userId);

        Task<bool> IsUserMemberOfGroupAsync(string userId, int groupId);

        Task<Group> CreateGroupAsync(string name, string ownerId, List<string> userIds);
    }
}
