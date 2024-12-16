// <copyright file="GroupStatistics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.DTO
{
    public class GroupStatisticsDto
    {
        /// <summary>Gets or sets the most watched genre.</summary>
        /// <value>The most watched genre.</value>
        public string MostWatchedGenre { get; set; }

        /// <summary>Gets or sets the total movies watched by group.</summary>
        /// <value>The total movies watched by group.</value>
        public int TotalMoviesWatchedByGroup { get; set; }

        /// <summary>
        /// Gets or sets the most popular decade.
        /// </summary>
        /// <value>
        /// The most popular decade.
        /// </value>
        public string MostPopularDecade { get; set; }
    }
}
