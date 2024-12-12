// <copyright file="ReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;
    using PanoramaApp.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace PanoramaApp.Services
    {
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Review>> GetReviewsForMovieAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddReviewAsync(int movieId, string userId, string content, int rating)
        {
            // Kontrollera att filmen existerar
            var movieExists = await _context.Movies.AnyAsync(m => m.Id == movieId);
            if (!movieExists)
            {
                throw new ArgumentException("The movie does not exist.");
            }

            // Skapa och spara recensionen
            var review = new Review
            {
                MovieId = movieId,
                UserId = userId,
                Content = content,
                Rating = rating,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
    }
}