using PanoramaApp.Data;
using PanoramaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Services
{
    public class MoviePrioritizer
{
    public static void SetPriority(Movie movie, int priority)
    {
        movie.Priority = priority;
    }

    public static List<Movie> GetPrioritizedMovies(List<Movie> movies)
    {
        return movies.OrderByDescending(m => m.Priority).ToList();
    }
}
}
