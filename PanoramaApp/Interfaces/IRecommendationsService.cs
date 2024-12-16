// <copyright file="IRecommendationsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using PanoramaApp.Models;

    public interface IRecommendationService
    {
        Task<List<Movie>> GetRecommendedMoviesAsync(ClaimsPrincipal user);
    }
}