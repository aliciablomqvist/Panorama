
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
        }
    }