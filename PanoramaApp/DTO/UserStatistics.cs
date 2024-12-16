// <copyright file="UserStatistics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.DTO
{
    public class UserStatisticsDto
    {
        /// <summary>
        /// Gets or sets the favorite genre.
        /// </summary>
        /// <value>
        /// The favorite genre.
        /// </value>
        public string FavoriteGenre { get; set; }

        /// <summary>
        /// Gets or sets the total movies watched.
        /// </summary>
        /// <value>
        /// The total movies watched.
        /// </value>
        public int TotalMoviesWatched { get; set; }

        /// <summary>
        /// Gets or sets the most watched decade.
        /// </summary>
        /// <value>
        /// The most watched decade.
        /// </value>
        public string MostWatchedDecade { get; set; }

        /// <summary>
        /// Gets or sets the genre count.
        /// </summary>
        /// <value>
        /// The genre count.
        /// </value>
        public Dictionary<string, int> GenreCount { get; set; }
    }
}
