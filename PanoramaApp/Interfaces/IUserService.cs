// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Interface for Users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the current user asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IdentityUser> GetCurrentUserAsync();

        /// <summary>
        /// Gets all users asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<IdentityUser>> GetAllUsersAsync();

        /// <summary>
        /// Gets the user by identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IdentityUser> GetUserByIdAsync(string userId);
    }
}
