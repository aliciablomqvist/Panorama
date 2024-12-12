using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using PanoramaApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Services
{
    public class MovieCalendarService : IMovieCalendarService
    {
        private readonly ApplicationDbContext _context;

        public MovieCalendarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<List<MovieCalendar>> GetScheduledMoviesAsync()
        {
            return await _context.MovieCalendars
                .Include(mc => mc.Movie)
                .ToListAsync();
        }

        public async Task ScheduleMovieAsync(int movieId, DateTime scheduledDate)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException("The movie does not exist.");
            }

            var calendarEntry = new MovieCalendar
            {
                MovieId = movieId,
                Date = scheduledDate,
            };

            _context.MovieCalendars.Add(calendarEntry);
            await _context.SaveChangesAsync();
        }
    }
}
