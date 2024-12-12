using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IMovieListService
    {
        Task AddToListAsync(string listName, int movieId, string userId);
        Task<List<MovieList>> GetListsByUserAsync(string userId);
        Task<MovieList> GetMovieListByIdAsync(int id);
        Task UpdateMoviePrioritiesAsync(List<MoviePriorityUpdate> updates);

        Task<List<Movie>> GetMoviesFromListAsync(string listName, string userId);

        Task<List<MovieList>> GetMovieListsForUserAsync(string userId);
        Task<MovieList> GetFavoritesListAsync(string userId);
    }
    public class MoviePriorityUpdate
    {
        public int Id { get; set; }
        public int Priority { get; set; }
    }
}
