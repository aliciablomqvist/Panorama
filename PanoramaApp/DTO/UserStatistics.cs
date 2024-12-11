namespace PanoramaApp.DTOs
{
    public class UserStatisticsDto
    {
        public string UserName { get; set; } = string.Empty;
        public int TotalMoviesWatched { get; set; }
        public string MostWatchedGenre { get; set; } = string.Empty;
        public int MostWatchedGenreCount { get; set; }
        public string MostWatchedDecade { get; set; } = string.Empty;
        public int MostWatchedDecadeCount { get; set; }
    }
}
