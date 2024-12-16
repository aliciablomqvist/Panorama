// <copyright file="Vote.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    /// <summary>
    /// Model for Voting.
    /// </summary>
    public class Vote
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; } = default!;

        public int GroupId { get; set; }

        public Group Group { get; set; } = default!;

        public string? UserId { get; set; }
    }
}
