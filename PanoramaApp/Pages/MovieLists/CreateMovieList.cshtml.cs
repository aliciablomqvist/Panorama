using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;

namespace PanoramaApp.Pages.MovieLists
{
    public class CreateMovieListModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateMovieListModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public bool IsShared { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var movieList = new MovieList
        {
            Name = Name,
            OwnerId = user.Id,
            IsShared = IsShared
        };

        _context.MovieLists.Add(movieList);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Movies/ViewMovieLists");
    }
}
}