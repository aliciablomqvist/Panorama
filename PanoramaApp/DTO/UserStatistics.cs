namespace PanoramaApp.DTO
{
    public class UserStatisticsDto
    {
        public string FavoriteGenre { get; set; }
        public int TotalMoviesWatched { get; set; }
        public string MostWatchedDecade { get; set; }
        public Dictionary<string, int> GenreCount { get; set; }
    
    }
}