// <copyright file="ShareList.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.MovieLists
{
    public class ShareModel : PageModel
    {
        private readonly IMovieListService _movieListService;

        public ShareModel(IMovieListService movieListService)
        {
            _movieListService = movieListService;
        }

        public MovieList MovieList { get; private set; }
        public string ShareableLink { get; private set; }

        public async Task<IActionResult> OnGetAsync(int listId)
        {

            MovieList = await _movieListService.GetMovieListByIdAsync(listId);

            if (MovieList == null)
            {
                return NotFound();
            }

            // Generera en delbar länk direkt med Url.Page
            ShareableLink = Url.Page(
                "/Movies/MovieListDetails",
                null,
                new { id = listId },
                Request.Scheme
            );

            return Page();
        }
    }
}
