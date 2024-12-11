using PanoramaApp.Data;
using PanoramaApp.DTO;
using PanoramaApp.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace PanoramaApp.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserStatisticsDto> GetUserStatisticsAsync(string userId)
        {
            var watchedMovies = await _context.MovieLists
                .Where(ml => ml.Name == "Watched" && ml.OwnerId == userId)
                .Include(ml => ml.Movies)
                .ThenInclude(mli => mli.Movie)
                .SelectMany(ml => ml.Movies.Select(mli => mli.Movie))
                .ToListAsync();

            var genreCount = watchedMovies
                .GroupBy(m => m.Genre)
                .ToDictionary(g => g.Key, g => g.Count());

            var mostWatchedDecade = watchedMovies
                .GroupBy(m => (m.ReleaseDate.Year / 10) * 10)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key.ToString() ?? "N/A";

            return new UserStatisticsDto
            {
                FavoriteGenre = genreCount.OrderByDescending(g => g.Value).FirstOrDefault().Key,
                TotalMoviesWatched = watchedMovies.Count,
                MostWatchedDecade = mostWatchedDecade,
                GenreCount = genreCount
            };
        }

    public async Task<GroupStatisticsDto> GetGroupStatisticsAsync(int groupId)
{
    // Hämta gruppmedlemmar
    var groupMembers = await _context.GroupMembers
        .Where(gm => gm.GroupId == groupId)
        .Select(gm => gm.UserId)
        .ToListAsync();

    if (!groupMembers.Any())
    {
        return new GroupStatisticsDto
        {
            MostWatchedGenre = "N/A",
            TotalMoviesWatchedByGroup = 0,
            MostPopularDecade = "N/A"
        };
    }

    // Hämta alla filmer från "Watched" och "Favorites" för alla gruppmedlemmar
    var watchedMovies = await _context.MovieLists
        .Where(ml => ml.Name == "Watched" && groupMembers.Contains(ml.OwnerId))
        .SelectMany(ml => ml.Movies.Select(mli => mli.Movie))
        .ToListAsync();

    var favoriteMovies = await _context.MovieLists
        .Where(ml => ml.Name == "My Favorites" && groupMembers.Contains(ml.OwnerId))
        .SelectMany(ml => ml.Movies.Select(mli => mli.Movie))
        .ToListAsync();

    var allMovies = watchedMovies.Concat(favoriteMovies).ToList();

    if (!allMovies.Any())
    {
        return new GroupStatisticsDto
        {
            MostWatchedGenre = "N/A",
            TotalMoviesWatchedByGroup = 0,
            MostPopularDecade = "N/A"
        };
    }

    // Beräkna statistik
    var mostWatchedGenre = allMovies
        .GroupBy(m => m.Genre)
        .OrderByDescending(g => g.Count())
        .FirstOrDefault()?.Key ?? "N/A";

var mostPopularDecade = allMovies
    .GroupBy(m => (m.ReleaseDate.Year / 10) * 10)
    .OrderByDescending(g => g.Count())
    .FirstOrDefault()?.Key.ToString() ?? "N/A";


    return new GroupStatisticsDto
    {
        MostWatchedGenre = mostWatchedGenre,
        TotalMoviesWatchedByGroup = allMovies.Count,
        MostPopularDecade = mostPopularDecade
    };
}
    }
}
