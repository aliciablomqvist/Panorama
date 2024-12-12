// <copyright file="AddToMovieList.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class AddMovieModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public AddMovieModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public MovieList MovieList { get; set; }

        [BindProperty]
        public List<int> SelectedMovieIds { get; set; } = new List<int>();

        public List<SelectListItem> MovieOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int listId)
        {
            Console.WriteLine($"OnGetAsync called with ID: {listId}");

            try
            {
                this.MovieList = await this.context.MovieLists
                    .Include(ml => ml.Movies)
                        .ThenInclude(mli => mli.Movie)
                    .FirstOrDefaultAsync(ml => ml.Id == listId);

                if (this.MovieList == null)
                {
                    Console.WriteLine($"MovieList not found for ID: {listId}");
                    return this.RedirectToPage("/Error");
                }

                var existingMovieIds = this.MovieList.Movies.Select(mlm => mlm.MovieId).ToList();

                var movies = await this.context.Movies
                    .Where(m => !existingMovieIds.Contains(m.Id))
                    .ToListAsync();

                this.MovieOptions = movies.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Title,
                }).ToList();

                Console.WriteLine($"Fetched MovieList: {this.MovieList.Name} with {this.MovieList.Movies.Count} movies.");
                return this.Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnGetAsync: {ex.Message}");
                return this.RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync(int listId)
        {
            this.MovieList = await this.context.MovieLists
                .Include(ml => ml.Movies)
                .FirstOrDefaultAsync(ml => ml.Id == listId);

            if (this.MovieList == null)
            {
                return this.NotFound();
            }

            if (this.SelectedMovieIds != null && this.SelectedMovieIds.Any())
            {
                foreach (var movieId in this.SelectedMovieIds)
                {
                    var movie = await this.context.Movies.FindAsync(movieId);

                    var movieListItem = new MovieListItem
                    {
                        MovieListId = listId,
                        MovieList = this.MovieList,
                        MovieId = movieId,
                        Movie = movie,
                    };
                    this.MovieList.Movies.Add(movieListItem);
                }

                await this.context.SaveChangesAsync();
            }

            return this.RedirectToPage("/Movies/MovieListDetails", new { id = listId });
        }
    }
}