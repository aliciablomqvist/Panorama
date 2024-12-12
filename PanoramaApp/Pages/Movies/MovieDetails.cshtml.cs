// <copyright file="MovieDetails.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Movies
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Services;

    public class MovieDetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ReviewService reviewService;

        public MovieDetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, ReviewService reviewService)
        {
            this.context = context;
            this.userManager = userManager;
            this.reviewService = reviewService;
        }

        public Movie Movie { get; set; }

        public IList<Review> Reviews { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.Movie = await this.context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (this.Movie == null)
            {
                return this.NotFound();
            }

            this.Reviews = await this.reviewService.GetReviewsForMovieAsync(id);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAddToFavoritesAsync(int movieId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.RedirectToPage("/Account/Login");
            }

            var userId = user.Id;
            var favoritesList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Name == "My Favorites" && ml.OwnerId == userId);

            if (favoritesList == null)
            {
                favoritesList = new MovieList
                {
                    Name = "My Favorites",
                    OwnerId = userId,
                };

                this.context.MovieLists.Add(favoritesList);
                await this.context.SaveChangesAsync();
            }

            if (!favoritesList.Movies.Any(mli => mli.MovieId == movieId))
            {
                var movieListItem = new MovieListItem
                {
                    MovieListId = favoritesList.Id,
                    MovieId = movieId,
                };
                favoritesList.Movies.Add(movieListItem);
                await this.context.SaveChangesAsync();
            }

            return this.RedirectToPage("/Movies/MovieDetails", new { id = movieId });
        }

        public async Task<IActionResult> OnPostMarkAsWatchedAsync(int movieId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.RedirectToPage("/Account/Login");
            }

            var userId = user.Id;

            var watchedList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Name == "Watched" && ml.OwnerId == userId);

            if (watchedList == null)
            {
                watchedList = new MovieList
                {
                    Name = "Watched",
                    OwnerId = userId,
                };

                this.context.MovieLists.Add(watchedList);
                await this.context.SaveChangesAsync();
            }

            if (!watchedList.Movies.Any(mli => mli.MovieId == movieId))
            {
                var movieListItem = new MovieListItem
                {
                    MovieListId = watchedList.Id,
                    MovieId = movieId,
                };

                watchedList.Movies.Add(movieListItem);
                await this.context.SaveChangesAsync();
            }

            return this.RedirectToPage("/Movies/MovieDetails", new { id = movieId });
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int movieId, string content, int rating)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToPage("/Account/Login");
            }

            await this.reviewService.AddReviewAsync(movieId, userId, content, rating);
            return this.RedirectToPage("/Movies/MovieDetails", new { id = movieId });
        }

        public string ConvertToEmbedUrl(string youtubeUrl)
        {
            if (string.IsNullOrEmpty(youtubeUrl))
            {
                return string.Empty;
            }

            try
            {
                var uri = new Uri(youtubeUrl);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                var videoId = query["v"]; // Extrahera 'v'-parametern från URL:en

                return videoId != null ? $"https://www.youtube.com/embed/{videoId}" : string.Empty;
            }
            catch
            {
                // Om något går fel, returnera en tom sträng
                return string.Empty;
            }
        }
    }
}