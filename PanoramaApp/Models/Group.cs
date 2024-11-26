namespace PanoramaApp.Models
{
public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Movie> Movies { get; set; } = new List<Movie>();
    public List<User> Members { get; set; } = new List<User>();
}
}