using PanoramaApp.Models;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IWinnerService
    {
        Task<(Movie? WinningMovie, int VoteCount)> GetWinningMovieAsync(int groupId);
    }
}
