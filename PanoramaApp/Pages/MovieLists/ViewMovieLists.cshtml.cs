using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PanoramaApp.Pages.MovieLists
{
    public class ViewMovieListsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ViewMovieListsModel> _logger;

        public ViewMovieListsModel(ApplicationDbContext context, ILogger<ViewMovieListsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<MovieList> MovieLists { get; set; } = new();
        public List<Group> Groups { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _logger.LogInformation("Fetching MovieLists and Groups for user {UserId}", userId);

            try
            {
                // Hämta film-listor som användaren äger eller som är delade med grupper användaren är medlem i
                MovieLists = await _context.MovieLists
                    .Include(ml => ml.Movies) // Hämta filmer i varje lista
                    .Include(ml => ml.SharedWithGroups) // Hämta grupper listan är delad med
                    .Where(ml => ml.OwnerId == userId || 
                                 ml.SharedWithGroups.Any(g => g.Members.Any(m => m.UserId == userId)))
                    .ToListAsync();

                if (MovieLists.Count == 0)
                {
                    _logger.LogWarning("No MovieLists found for user {UserId}", userId);
                }
                else
                {
                    _logger.LogInformation("Fetched {Count} MovieLists for user {UserId}", MovieLists.Count, userId);
                }

                // Hämta grupper där användaren är medlem
                Groups = await _context.Groups
                    .Include(g => g.Members)
                        .ThenInclude(m => m.User)
                    .Include(g => g.Movies)
                    .Where(g => g.Members.Any(m => m.UserId == userId))
                    .ToListAsync();

                if (Groups.Count == 0)
                {
                    _logger.LogWarning("No groups found for user {UserId}", userId);
                }
                else
                {
                    _logger.LogInformation("Fetched {Count} groups for user {UserId}", Groups.Count, userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching MovieLists or Groups.");
            }
        }
    }
}
