// <copyright file="ViewWinner.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Interfaces;
using PanoramaApp.Services;
using PanoramaApp.Models;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Groups
{
    public class ViewWinnerModel : PageModel
    {
        private readonly IWinnerService _winnerService;

        public ViewWinnerModel(IWinnerService winnerService)
        {
            _winnerService = winnerService;
        }

        public Group Group { get; private set; } = new();
        public Movie? WinningMovie { get; private set; }
        public int WinningMovieVoteCount { get; private set; } = 0;

        public async Task OnGetAsync(int id)
        {
            var (winningMovie, voteCount) = await _winnerService.GetWinningMovieAsync(id);

            WinningMovie = winningMovie;
            WinningMovieVoteCount = voteCount;
        }
    }
}
