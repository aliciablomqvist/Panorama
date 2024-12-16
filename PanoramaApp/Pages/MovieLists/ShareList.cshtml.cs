// <copyright file="ShareList.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class ShareListModel : PageModel
    {
        private readonly IMovieListService movieListService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareListModel"/> class.
        /// </summary>
        /// <param name="movieListService">The movie list service.</param>
        public ShareListModel(IMovieListService movieListService)
        {
            this.movieListService = movieListService;
        }

        public MovieList MovieList { get; private set; }

        public string ShareableLink { get; private set; }

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync(int listId)
        {
            this.MovieList = await this.movieListService.GetMovieListByIdAsync(listId);

            if (this.MovieList == null)
            {
                return this.NotFound();
            }

            this.ShareableLink = this.Url.Page(
                "/Movies/MovieListDetails",
                null,
                new { id = listId },
                this.Request.Scheme);

            return this.Page();
        }
    }
}
