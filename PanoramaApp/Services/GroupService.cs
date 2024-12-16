using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;

        public GroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetGroupsForUserAsync(string userId)
        {
            return await _context.Groups
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .Include(g => g.Movies)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }
        public async Task<List<Group>> GetUserGroupsAsync(string userId)
        {
            return await _context.Groups
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task AddMovieListToGroupsAsync(MovieList movieList, List<int> groupIds)
        {
            var groups = await _context.Groups
                .Where(g => groupIds.Contains(g.Id))
                .ToListAsync();

            foreach (var group in groups)
            {
                group.MovieLists.Add(movieList);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await _context.Groups
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.Id == groupId);
 }
                public async Task<Group> GetGroupWithMoviesAsync(int groupId,string userId)
        {
            return await _context.Groups
                .Include(g => g.Movies)
                   .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync()
                .FirstOrDefaultAsync(g => g.Id == groupId);

        }
        }
}
 
