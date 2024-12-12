// <copyright file="MoviePrioritizer.cs" company="PlaceholderCompany">
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

    public static class MoviePrioritizer
    {
        public static List<Movie> Prioritize(List<Movie> movies, int movieListId, int movieId, int priority)
        {
            var movie = movies.FirstOrDefault(m => m.Id == movieId && m.MovieLists.Any(ml => ml.Id == movieListId));
            if (movie != null)
            {
                movie.Priority = priority;
            }

            return movies
                   .Where(m => m.MovieLists.Any(ml => ml.Id == movieListId))
                   .OrderByDescending(m => m.Priority)
                   .ToList();
        }

        public static List<Movie> GetPrioritizedMovies(List<Movie> movies)
        {
            return movies.OrderByDescending(m => m.Priority).ToList();
        }
    }
}