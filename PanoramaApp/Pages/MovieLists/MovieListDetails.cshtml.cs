// <copyright file="MovieListDetails.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class MovieListDetailsModel : PageModel
    {
        private readonly IMovieListService movieListService;

        public MovieListDetailsModel(IMovieListService movieListService)
        {
            this.movieListService = movieListService;
        }

        public MovieList MovieList { get; set; } = default!;

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.MovieList = await this.movieListService.GetMovieListByIdAsync(id);

            if (this.MovieList == null)
            {
                return this.RedirectToPage("/Error");
            }

            return this.Page();
        }

        /// <summary>
        /// Called when [post save priorities asynchronous].
        /// </summary>
        /// <param name="updates">The updates.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSavePrioritiesAsync([FromBody] List<MoviePriorityUpdate> updates)
        {
            if (updates == null || !updates.Any())
            {
                return this.BadRequest("Invalid data received.");
            }

            await this.movieListService.UpdateMoviePrioritiesAsync(updates);
            return new JsonResult(new { success = true });
        }
    }
}
