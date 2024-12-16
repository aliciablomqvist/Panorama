// <copyright file="ExploreMovies.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    /// <summary>
    /// Represents a movie exploration page in the application.
    /// Handles movie-related data and operations.
    /// </summary>
    public class ExploreMoviesModel : PageModel
    {
        private readonly IMovieService movieService;

        public ExploreMoviesModel(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public async Task OnGetAsync()
        {
            this.Movies = await this.movieService.GetMoviesAsync();
        }
    }
}