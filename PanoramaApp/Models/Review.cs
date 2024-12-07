using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Movie Movie { get; set; }
        public IdentityUser User { get; set; }
    }
}