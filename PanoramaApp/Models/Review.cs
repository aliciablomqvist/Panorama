using Microsoft.AspNetCore.Identity;
namespace PanoramaApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // Betyg 1-5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int MovieId { get; set; }
        public Movie Movie { get; set; } // Navigeringsrelation

        public string UserId { get; set; }
        public IdentityUser User { get; set; } // Navigeringsrelation
    }
}
