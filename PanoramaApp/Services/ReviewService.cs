using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Data;
using PanoramaApp.Models;
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
            Rating = rating
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }
}
}