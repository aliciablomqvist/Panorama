namespace PanoramaApp.Models
{public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Votes { get; set; } = 0; 

        public int? GroupId { get; set; }
    public Group? Group { get; set; }
}
}