using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Services
{
    public class MovieListService : IMovieListService
    {
        private readonly ApplicationDbContext _context;

        public MovieListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToListAsync(string listName, int movieId, string userId)
        {
            var list = await _context.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Name == listName && ml.OwnerId == userId);

            if (list == null)
            {
                list = new MovieList { Name = listName, OwnerId = userId };
                _context.MovieLists.Add(list);
                await _context.SaveChangesAsync();
            }

            if (!list.Movies.Any(mli => mli.MovieId == movieId))
            {
                var movieListItem = new MovieListItem { MovieListId = list.Id, MovieId = movieId };
                list.Movies.Add(movieListItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MovieList>> GetListsByUserAsync(string userId)
        {
            return await _context.MovieLists
                .Where(ml => ml.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<MovieList> GetMovieListByIdAsync(int id)
        {
            return await _context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Id == id);
        }

        public async Task UpdateMoviePrioritiesAsync(List<MoviePriorityUpdate> updates)
        {
            foreach (var update in updates)
            {
                var movie = await _context.Movies
                    .FirstOrDefaultAsync(m => m.Id == update.Id);

                if (movie != null)
                {
                    movie.Priority = update.Priority;
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<Movie>> GetMoviesFromListAsync(string listName, string userId)
        {
            var movieList = await _context.MovieLists
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .FirstOrDefaultAsync(ml => ml.Name == listName && ml.OwnerId == userId);

            return movieList?.Movies.Select(mli => mli.Movie).ToList() ?? new List<Movie>();
        }
    }
}