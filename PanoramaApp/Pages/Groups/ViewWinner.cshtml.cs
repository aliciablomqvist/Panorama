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

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewWinnerModel"/> class.
        /// </summary>
        /// <param name="winnerService">The winner service.</param>
        public ViewWinnerModel(IWinnerService winnerService)
        {
            this.winnerService = winnerService;
        }

        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public Group Group { get; private set; } = new ();

        /// <summary>
        /// Gets the winning movie.
        /// </summary>
        /// <value>
        /// The winning movie.
        /// </value>
        public Movie? WinningMovie { get; private set; }

        /// <summary>
        /// Gets the winning movie vote count.
        /// </summary>
        /// <value>
        /// The winning movie vote count.
        /// </value>
        public int WinningMovieVoteCount { get; private set; } = 0;

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task OnGetAsync(int id)
        {
            var (winningMovie, voteCount) = await this.winnerService.GetWinningMovieAsync(id);

            this.WinningMovie = winningMovie;
            this.WinningMovieVoteCount = voteCount;
        }
    }
}
