// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using PanoramaApp.Interfaces;

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityUser> GetCurrentUserAsync()
        {
            return await this.userManager.GetUserAsync(ClaimsPrincipal.Current);
        }

        public async Task<List<IdentityUser>> GetAllUsersAsync()
        {
            return await Task.Run(() => this.userManager.Users.ToList());
        }

        public async Task<IdentityUser> GetUserByIdAsync(string userId)
        {
            return await this.userManager.FindByIdAsync(userId);
        }
    }
}