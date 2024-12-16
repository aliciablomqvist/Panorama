// <copyright file="GroupService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GroupService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds the movie list to groups asynchronous.
        /// </summary>
        /// <param name="movieList">The movie list.</param>
        /// <param name="groupIds">The group ids.</param>
        public async Task AddMovieListToGroupsAsync(MovieList movieList, List<int> groupIds)
        {
            var groups = await this.context.Groups
                .Where(g => groupIds.Contains(g.Id))
                .ToListAsync();

            foreach (var group in groups)
            {
                group.MovieLists.Add(movieList);
            }

            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the group by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await this.context.Groups
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }

        /// <summary>
        /// Gets the group with movies asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<Group> GetGroupWithMoviesAsync(int groupId, string userId)
        {
            var groups = await this.context.Groups
                .Include(g => g.Movies)
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();

            return groups.FirstOrDefault(g => g.Id == groupId);
        }

        /// <summary>
        /// Gets the groups for user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<Group>> GetGroupsForUserAsync(string userId)
        {
            return await this.context.Groups
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        /// <summary>
        /// Gets the detailed groups for user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<Group>> GetDetailedGroupsForUserAsync(string userId)
        {
            return await this.context.Groups
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .Include(g => g.Movies)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        /// <summary>
        /// Determines whether [is user member of group asynchronous] [the specified user identifier].
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is user member of group asynchronous] [the specified user identifier]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsUserMemberOfGroupAsync(string userId, int groupId)
        {
            return await this.context.GroupMembers
                .AnyAsync(m => m.GroupId == groupId && m.UserId == userId);
        }

        /// <summary>
        /// Creates the group asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="userIds">The user ids.</param>
        /// <returns></returns>
        public async Task<Group> CreateGroupAsync(string name, string ownerId, List<string> userIds)
        {
            var newGroup = new Group
            {
                Name = name,
                OwnerId = ownerId,
            };

            this.context.Groups.Add(newGroup);
            await this.context.SaveChangesAsync();

            var groupMembers = userIds.Select(userId => new GroupMember
            {
                GroupId = newGroup.Id,
                UserId = userId,
            });

            this.context.GroupMembers.AddRange(groupMembers);
            await this.context.SaveChangesAsync();

            return newGroup;
        }

        /// <summary>
        /// Gets the user groups asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<Group>> GetUserGroupsAsync(string userId)
        {
            return await this.context.Groups
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        /// <summary>
        /// Gets the specific group by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        public async Task<Group?> GetSpecificGroupByIdAsync(int groupId)
        {
            return await this.context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
        }
    }
}
