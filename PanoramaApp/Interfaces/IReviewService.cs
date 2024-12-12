using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IReviewService
    {
        Task<IList<Review>> GetReviewsForMovieAsync(int movieId);
        Task AddReviewAsync(int movieId, string userId, string content, int rating);
    }
}