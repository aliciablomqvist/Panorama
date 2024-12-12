// Interface/IMovieService.cs
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync();
    }
}