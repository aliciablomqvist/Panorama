using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Services
{
    public class WinnerService : IWinnerService
    {
        private readonly IGroupService _groupService;
        private readonly IVoteService _voteService;

        private readonly IUserService _userService;

public WinnerService(IGroupService groupService, IVoteService voteService, IUserService userService)
{
    _groupService = groupService;
    _voteService = voteService;
    _userService = userService;
}

public async Task<(Movie? WinningMovie, int VoteCount)> GetWinningMovieAsync(int groupId)
{
    var user = await _userService.GetCurrentUserAsync();
    var userId = user.Id;

    var group = await _groupService.GetGroupWithMoviesAsync(groupId, userId);

    if (group == null || !group.Movies.Any())
    {
        return (null, 0);
    }

    var voteCounts = await _voteService.GetVoteCountsForGroupAsync(groupId);

    var winningMovie = group.Movies
        .OrderByDescending(m => voteCounts.ContainsKey(m.Id) ? voteCounts[m.Id] : 0)
        .FirstOrDefault();

    var winningVoteCount = winningMovie != null && voteCounts.ContainsKey(winningMovie.Id)
        ? voteCounts[winningMovie.Id]
        : 0;

    return (winningMovie, winningVoteCount);
}

    }
}
