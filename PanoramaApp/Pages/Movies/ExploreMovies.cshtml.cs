// <copyright file="ExploreMovies.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Services;
    using PanoramaApp.Interfaces;

    /// <summary>
    /// Represents a movie exploration page in the application.
    /// Handles movie-related data and operations.
    /// </summary>
    public class ExploreMoviesModel : PageModel
    {
        private readonly IMovieService _movieService;

        public ExploreMoviesModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public async Task OnGetAsync()
        {
            Movies = await _movieService.GetMoviesAsync();
        }
    }
}