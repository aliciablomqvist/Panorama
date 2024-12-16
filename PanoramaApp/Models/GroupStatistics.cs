// <copyright file="GroupStatistics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    /// <summary>
    /// Model for group statistics
    /// </summary>
    /// <seealso cref="PanoramaApp.Models.Statistics" />
    public class GroupStatistics : Statistics
    {
        public string GroupName { get; set; } = string.Empty;

        public int TotalMoviesWatchedByGroup { get; set; }

        public Dictionary<string, int> MemberContribution { get; set; } = new ();
    }
}
