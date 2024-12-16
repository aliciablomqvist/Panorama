// <copyright file="MovieList.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using Microsoft.AspNetCore.Identity;

    public class MovieList
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string OwnerId { get; set; }

        public IdentityUser? Owner { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsShared { get; set; } = false;

        public ICollection<MovieListItem> Movies { get; set; } = new List<MovieListItem>();

        public ICollection<Group> SharedWithGroups { get; set; } = new List<Group>();
    }

    public class MovieListItem
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public Movie? Movie { get; set; }

        public int MovieListId { get; set; }

        public MovieList? MovieList { get; set; }
    }
}
