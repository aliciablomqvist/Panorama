// <copyright file="IRecommendationsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for recommendations
    /// </summary>
    public interface IRecommendationService
    {
        /// <summary>
        /// Gets the recommended movies asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task<List<Movie>> GetRecommendedMoviesAsync(ClaimsPrincipal user);
    }
}
