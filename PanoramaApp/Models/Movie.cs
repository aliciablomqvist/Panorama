// <copyright file="Movie.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    /// <summary>
    /// Model for movies.
    /// </summary>
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }

        public int Votes { get; set; } = 0;

        public int? GroupId { get; set; }

        public Group? Group { get; set; }

        public ICollection<MovieListItem> MovieListItems { get; set; }

        public ICollection<MovieList> MovieLists { get; set; } = new List<MovieList>();

        public string TrailerUrl { get; set; }

        public int? Priority { get; set; }

        public int? TmdbId { get; set; }
    }
}
