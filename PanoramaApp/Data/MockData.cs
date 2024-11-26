using PanoramaApp.Models;
namespace PanoramaApp.Data
{
    public static class MockData
{
    public static List<Movie> Movies = new List<Movie>
    {
        new Movie { Id = 1, Title = "The Shawshank Redemption" },
        new Movie { Id = 2, Title = "The Godfather" },
        new Movie { Id = 3, Title = "The Dark Knight" }
    };

    public static List<User> Users = new List<User>
    {
        new User { Id = 1, Name = "Alice" },
        new User { Id = 2, Name = "Bob" },
        new User { Id = 3, Name = "Charlie" }
    };


    public static List<Group> Groups = new List<Group>
    {
        new Group
        {
            Id = 1,
            Name = "Film Lovers",
            Movies = new List<Movie> { Movies[0], Movies[1] },
            Members = new List<User> { Users[0], Users[1] }
        }
    };
}
}