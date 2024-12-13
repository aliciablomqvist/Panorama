using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Services
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDbContext _context;

        public VoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddVoteAsync(int groupId, int movieId, string userId)
        {
            var vote = new Vote
            {
                MovieId = movieId,
                GroupId = groupId,
                UserId = userId,
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetVotesForMovieAsync(int movieId)
        {
            return await _context.Votes
                .Where(v => v.MovieId == movieId)
                .CountAsync();
 }
                 public async Task<Dictionary<int, int>> GetVoteCountsForGroupAsync(int groupId)
        {
            var votes = await _context.Votes
                .Where(v => v.GroupId == groupId)
                .GroupBy(v => v.MovieId)
                .Select(g => new { MovieId = g.Key, VoteCount = g.Count() })
                .ToDictionaryAsync(g => g.MovieId, g => g.VoteCount);

            return votes;
        }
    }
    }
