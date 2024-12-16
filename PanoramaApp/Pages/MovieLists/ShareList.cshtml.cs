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

        public ShareListModel(IMovieListService movieListService)
        {
            this.movieListService = movieListService;
        }

        public MovieList MovieList { get; private set; }

        public string ShareableLink { get; private set; }

        public async Task<IActionResult> OnGetAsync(int listId)
        {
            this.MovieList = await this.movieListService.GetMovieListByIdAsync(listId);

            if (this.MovieList == null)
            {
                return this.NotFound();
            }

            // Generera en delbar länk direkt med Url.Page
            this.ShareableLink = this.Url.Page(
                "/Movies/MovieListDetails",
                null,
                new { id = listId },
                this.Request.Scheme);

            return this.Page();
        }
    }
}