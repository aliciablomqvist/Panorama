// Interface/IMovieService.cs
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<List<Movie>> GetAvailableMoviesForGroupAsync(int groupId);
        Task AssignMoviesToGroupAsync(int groupId, List<int> movieIds);
    }
}