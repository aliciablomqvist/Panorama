using PanoramaApp.Data;
using PanoramaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Services
{
public static class MoviePrioritizer
{
   public static List<Movie> Prioritize(List<Movie> movies, int movieId, int priority)
{
    var movie = movies.FirstOrDefault(m => m.Id == movieId);
    if (movie != null)
    {
        movie.Priority = priority;
    }

    return movies.OrderByDescending(m => m.Priority).ToList();
}
    public static List<Movie> GetPrioritizedMovies(List<Movie> movies)
    {
        return movies.OrderByDescending(m => m.Priority).ToList();
    }
}
}
