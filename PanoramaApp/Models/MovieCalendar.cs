using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{
    public class MovieCalendar
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}
}
