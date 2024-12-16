// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<IdentityUser> GetCurrentUserAsync();

        Task<List<IdentityUser>> GetAllUsersAsync();

        Task<IdentityUser> GetUserByIdAsync(string userId);
    }
}
