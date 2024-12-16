using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IUserService
    {
        Task<IdentityUser> GetCurrentUserAsync();
        Task<List<IdentityUser>> GetAllUsersAsync();
    }
}
