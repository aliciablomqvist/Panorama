// <copyright file="MovieCalendar.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using Microsoft.AspNetCore.Identity;

    public class MovieCalendar
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public List<(DateTime Date, Movie Movie)> EntriesValue = new ();

        public void AddMovie(Movie movie, DateTime date)
        {
            this.EntriesValue.Add((date, movie));
        }

        public List<(DateTime Date, Movie Movie)> Entries => this.EntriesValue;
    }
}
