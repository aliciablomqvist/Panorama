// <copyright file="MovieSorter.cs" company="PlaceholderCompany">
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
