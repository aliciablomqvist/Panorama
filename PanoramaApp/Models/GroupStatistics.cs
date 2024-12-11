namespace PanoramaApp.Models
{
    public class GroupStatistics : Statistics
    {
        public string GroupName { get; set; } = string.Empty;
        public int TotalMoviesWatchedByGroup { get; set; }
        public Dictionary<string, int> MemberContribution { get; set; } = new();
    }
}
