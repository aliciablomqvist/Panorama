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

        public async Task<IList<Review>> GetReviewsForMovieAsync(int movieId)
        {
            return await this.context.Reviews
                .Where(r => r.MovieId == movieId)
                 .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

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