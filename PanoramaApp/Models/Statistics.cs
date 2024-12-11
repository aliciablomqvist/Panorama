namespace PanoramaApp.Models
{
    public class Statistics
    {
        public string MostWatchedGenre { get; set; } = "Unknown";
        public int MostWatchedDecade { get; set; } = 0;
        public Dictionary<string, int> GenreCounts { get; set; } = new();
        public Dictionary<int, int> DecadeCounts { get; set; } = new();
    }
}
