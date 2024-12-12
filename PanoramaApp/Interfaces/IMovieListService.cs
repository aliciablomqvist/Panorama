using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IMovieListService
    {
        Task AddToListAsync(string listName, int movieId, string userId);
        Task<List<MovieList>> GetListsByUserAsync(string userId);
    }
      }
