using PanoramaApp.Models;
using PanoramaApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IMovieCalendarService
    {
        Task<List<Movie>> GetAllMoviesAsync();
        Task<List<MovieCalendar>> GetScheduledMoviesAsync();
        Task ScheduleMovieAsync(int movieId, DateTime scheduledDate);
    }
}
