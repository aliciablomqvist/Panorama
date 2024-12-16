
using PanoramaApp.Models;
using PanoramaApp.Data;
using PanoramaApp.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace PanoramaApp.Services
{
        public class MovieService : IMovieService
        {
            private readonly ApplicationDbContext _context;

            public MovieService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Movie>> GetMoviesAsync()
            {
                return await _context.Movies
                    .OrderByDescending(m => m.ReleaseDate)
                    .ToListAsync();
            }
        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movies
                 .Include(m => m.MovieListItems)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<List<Movie>> GetAvailableMoviesForGroupAsync(int groupId)
        {
            var assignedMovieIds = await _context.Movies
                .Where(m => m.GroupId == groupId)
                .Select(m => m.Id)
                .ToListAsync();

            return await _context.Movies
                .Where(m => !assignedMovieIds.Contains(m.Id))
                .ToListAsync();
        }

        public async Task AssignMoviesToGroupAsync(int groupId, List<int> movieIds)
        {
            var movies = await _context.Movies
                .Where(m => movieIds.Contains(m.Id))
                .ToListAsync();

            foreach (var movie in movies)
            {
                movie.GroupId = groupId;
            }

            _context.Movies.UpdateRange(movies);
            await _context.SaveChangesAsync();
        }
    }
    }