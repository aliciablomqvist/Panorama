// <copyright file="MovieListDetails.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Interfaces;

    public class MovieListDetailsModel : PageModel
    {
        private readonly IMovieListService _movieListService;

        public MovieListDetailsModel(IMovieListService movieListService)
        {
            _movieListService = movieListService;
        }

        public MovieList MovieList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            MovieList = await _movieListService.GetMovieListByIdAsync(id);

            if (MovieList == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSavePrioritiesAsync([FromBody] List<MoviePriorityUpdate> updates)
        {
            if (updates == null || !updates.Any())
            {
                return BadRequest("Invalid data received.");
            }

            await _movieListService.UpdateMoviePrioritiesAsync(updates);
            return new JsonResult(new { success = true });
        }
    }
}