// <copyright file="Group.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Model for groups.
    /// </summary>
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();

        public List<Vote> Votes { get; set; } = new List<Vote>();

        public ICollection<MovieList> MovieLists { get; set; } = new List<MovieList>();

        public string OwnerId { get; set; }
    }
}
