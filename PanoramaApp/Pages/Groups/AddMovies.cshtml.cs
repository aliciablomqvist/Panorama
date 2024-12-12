// <copyright file="AddMoviesModel.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;

namespace PanoramaApp.Pages.Groups
{
    public class AddMoviesModel : PageModel
{
    private readonly ApplicationDbContext context;

    public AddMoviesModel(ApplicationDbContext context)
    {
        this.context = context;
    }

    public Group Group { get; set; } = default!;

    public List<Movie> AvailableMovies { get; set; } = new List<Movie>();

    [BindProperty]
    public int GroupId { get; set; }

    [BindProperty]
    public List<int> SelectedMovies { get; set; } = new List<int>();

    public async Task OnGetAsync(int groupId)
    {
        this.Group = await this.context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

        if (this.Group != null)
        {
            var assignedMovieIds = await this.context.Movies
                .Where(m => m.GroupId == groupId)
                .Select(m => m.Id)
                .ToListAsync();

            this.AvailableMovies = await this.context.Movies
                .Where(m => !assignedMovieIds.Contains(m.Id))
                .ToListAsync();
        }
    }

    public async Task<IActionResult> OnPostAddMoviesAsync()
    {
        var group = await this.context.Groups.FirstOrDefaultAsync(g => g.Id == this.GroupId);
        if (group == null)
        {
            return this.RedirectToPage("/Error");
        }

        foreach (var movieId in this.SelectedMovies)
        {
            var movie = await this.context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie != null)
            {
                movie.GroupId = group.Id;
                this.context.Movies.Update(movie);
            }
        }

        await this.context.SaveChangesAsync();
        return this.RedirectToPage("/Groups/ViewGroups");
    }
}
}