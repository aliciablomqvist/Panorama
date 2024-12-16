// <copyright file="ViewWinner.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.RazorPages;

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class ViewWinnerModel : PageModel
    {
        private readonly IWinnerService winnerService;

        public ViewWinnerModel(IWinnerService winnerService)
        {
            this.winnerService = winnerService;
        }

        public Group Group { get; private set; } = new ();

        public Movie? WinningMovie { get; private set; }

        public int WinningMovieVoteCount { get; private set; } = 0;

        public async Task OnGetAsync(int id)
        {
            var (winningMovie, voteCount) = await this.winnerService.GetWinningMovieAsync(id);

            this.WinningMovie = winningMovie;
            this.WinningMovieVoteCount = voteCount;
        }
    }
}
