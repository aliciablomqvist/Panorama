using PanoramaApp.Data;
using PanoramaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Services
{
public static class MovieSorter
{
    public static List<Movie> SortByList(List<Movie> movies, string sortBy)
    {
        return sortBy switch
        {
            "Title" => movies.OrderBy(m => m.Title).ToList(),
            "ReleaseDate" => movies.OrderBy(m => m.ReleaseDate).ToList(),
            _ => movies
        };
    }
}
}
