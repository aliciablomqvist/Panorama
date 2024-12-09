using PanoramaApp.Data;
using PanoramaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Services
{
    public class ReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

    public async Task AddReviewAsync(int movieId, string userId, string content, int rating)
{
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
        public async Task<IList<Review>> GetReviewsForMovieAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .Include(r => r.User)
                .ToListAsync();
        }
    }
}
