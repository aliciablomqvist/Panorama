// <copyright file="IGroupService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;
    using PanoramaApp.Services;

    /// <summary>
    /// Interface for group related operations
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Gets the groups for user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<Group>> GetGroupsForUserAsync(string userId);

        /// <summary>
        /// Gets the detailed groups for user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<Group>> GetDetailedGroupsForUserAsync(string userId);

        /// <summary>
        /// Gets the user groups asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<Group>> GetUserGroupsAsync(string userId);

        /// <summary>
        /// Gets the specific group by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<Group?> GetSpecificGroupByIdAsync(int groupId);

        /// <summary>
        /// Adds the movie list to groups asynchronous.
        /// </summary>
        /// <param name="movieList">The movie list.</param>
        /// <param name="groupIds">The group ids.</param>
        /// <returns></returns>
        Task AddMovieListToGroupsAsync(MovieList movieList, List<int> groupIds);

        /// <summary>
        /// Gets the group by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<Group> GetGroupByIdAsync(int groupId);

        /// <summary>
        /// Gets the group with movies asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<Group> GetGroupWithMoviesAsync(int groupId, string userId);

        /// <summary>
        /// Determines whether [is user member of group asynchronous] [the specified user identifier].
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<bool> IsUserMemberOfGroupAsync(string userId, int groupId);

        /// <summary>
        /// Creates the group asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="userIds">The user ids.</param>
        /// <returns></returns>
        Task<Group> CreateGroupAsync(string name, string ownerId, List<string> userIds);
    }
}
