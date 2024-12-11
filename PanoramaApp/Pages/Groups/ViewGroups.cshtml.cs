using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Groups
{
    public class ViewGroupsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewGroupsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Group> Groups { get; set; } = new List<Group>();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Filtrera grupper där användaren är medlem
            Groups = await _context.Groups
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .Include(g => g.Movies)
                .Where(g => g.Members.Any(m => m.UserId == userId)) // Visa endast grupper där användaren är medlem
                .ToListAsync();
        }
    }
}
