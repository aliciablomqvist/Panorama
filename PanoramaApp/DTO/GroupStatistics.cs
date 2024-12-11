namespace PanoramaApp.DTOs
{
    public class GroupStatisticsDto
    {
        public string GroupName { get; set; } = string.Empty;
        public int TotalMoviesWatchedByGroup { get; set; }
        public string MostWatchedGenre { get; set; } = string.Empty;
        public int MostWatchedGenreCount { get; set; }
        public string MostWatchedDecade { get; set; } = string.Empty;
        public int MostWatchedDecadeCount { get; set; }
        public Dictionary<string, int> MemberContribution { get; set; } = new();
    }
}
