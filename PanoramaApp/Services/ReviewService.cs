// <copyright file="ReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class ReviewService
    {
        private readonly ApplicationDbContext context;

        public ReviewService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddReviewAsync(int movieId, string userId, string content, int rating)
        {
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

        public async Task<IList<Review>> GetReviewsForMovieAsync(int movieId)
        {
            return await this.context.Reviews
                .Where(r => r.MovieId == movieId)
                .Include(r => r.User)
                .ToListAsync();
        }
    }
}