using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IVoteService
    {
        Task AddVoteAsync(int groupId, int movieId, string userId);
        Task<int> GetVotesForMovieAsync(int movieId);

        Task<Dictionary<int, int>> GetVoteCountsForGroupAsync(int groupId);
    }
}
