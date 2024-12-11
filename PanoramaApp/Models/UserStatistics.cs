using PanoramaApp.Services;

namespace PanoramaApp.Models
{
   public class UserStatistics : Statistics
    {
        public string UserName { get; set; } = string.Empty;
        public int TotalMoviesWatched { get; set; }
    }
}