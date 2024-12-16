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

        /// <summary>
        /// Initializes a new instance of the <see cref="ExploreMoviesModel"/> class.
        /// </summary>
        /// <param name="movieService">The movie service.</param>
        public ExploreMoviesModel(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        /// <summary>
        /// Gets or sets the movies.
        /// </summary>
        /// <value>
        /// The movies.
        /// </value>
        public List<Movie> Movies { get; set; } = new List<Movie>();

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnGetAsync()
        {
            this.Movies = await this.movieService.GetMoviesAsync();
        }
    }
}
