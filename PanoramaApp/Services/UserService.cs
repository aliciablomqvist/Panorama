using Microsoft.AspNetCore.Identity;
using PanoramaApp.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace PanoramaApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(ClaimsPrincipal.Current);
        }

        public async Task<List<IdentityUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}
