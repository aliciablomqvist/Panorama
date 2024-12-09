using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{public class MovieCalendar
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public List<(DateTime Date, Movie Movie)> _entries = new();

public void AddMovie(Movie movie, DateTime date)
{
    _entries.Add((date, movie));
}


    public List<(DateTime Date, Movie Movie)> Entries => _entries;
}
}