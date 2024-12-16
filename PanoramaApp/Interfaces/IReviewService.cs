// <copyright file="IReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PanoramaApp.Models;

    public interface IReviewService
    {
        Task<IList<Review>> GetReviewsForMovieAsync(int movieId);

        Task AddReviewAsync(int movieId, string userId, string content, int rating);
    }
}