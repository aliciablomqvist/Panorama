using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ViewMovieListsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ViewMovieListsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<MovieList> MovieLists { get; set; } = new List<MovieList>();

    public async Task OnGetAsync()
    {
        MovieLists = await _context.MovieLists.ToListAsync();
    }
}
