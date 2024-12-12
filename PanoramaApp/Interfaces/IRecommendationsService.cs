using PanoramaApp.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<Movie>> GetRecommendedMoviesAsync(ClaimsPrincipal user);
    }
}
