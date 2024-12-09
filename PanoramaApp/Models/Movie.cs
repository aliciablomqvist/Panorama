namespace PanoramaApp.Models
{
    public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

       public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; }

    public int Votes { get; set; } = 0; 

        public int? GroupId { get; set; }
    public Group? Group { get; set; }

    public ICollection<MovieListItem> MovieListItems { get; set; }
 public ICollection<MovieList> MovieLists { get; set; } = new List<MovieList>();

  public string TrailerUrl { get; set; } 
    public int Priority { get; set; } 
    }
}