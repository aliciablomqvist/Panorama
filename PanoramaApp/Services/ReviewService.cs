// <copyright file="ReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext context;

        public ReviewService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the reviews for movie asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        public async Task<IList<Review>> GetReviewsForMovieAsync(int movieId)
        {
            return await this.context.Reviews
                .Where(r => r.MovieId == movieId)
                 .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Adds the review asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="rating">The rating.</param>
        /// <exception cref="System.ArgumentException">The movie does not exist.</exception>
        public async Task AddReviewAsync(int movieId, string userId, string content, int rating)
        {
            var movieExists = await this.context.Movies.AnyAsync(m => m.Id == movieId);
            if (!movieExists)
            {
                throw new ArgumentException("The movie does not exist.");
            }

            var review = new Review
            {
                MovieId = movieId,
                UserId = userId,
                Content = content,
                Rating = rating,
                CreatedAt = DateTime.UtcNow,
            };

            this.context.Reviews.Add(review);
            await this.context.SaveChangesAsync();
        }
    }
}
