// <copyright file="IReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for reviews.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Gets the reviews for movie asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IList<Review>> GetReviewsForMovieAsync(int movieId);

        /// <summary>
        /// Adds the review asynchronous.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddReviewAsync(int movieId, string userId, string content, int rating);
    }
}
