// <copyright file="UserStatistics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using PanoramaApp.Services;

    public class UserStatistics : Statistics
    {
        public string UserName { get; set; } = string.Empty;

        public int TotalMoviesWatched { get; set; }
    }
}