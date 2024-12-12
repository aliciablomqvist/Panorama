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
    using PanoramaApp.Interfaces;
    using PanoramaApp.Services;

    public class AddMovieModel : PageModel
    {
        private readonly IMovieListService _movieListService;
        private readonly UserManager<IdentityUser> _userManager;

        public AddMovieModel(IMovieListService movieListService, UserManager<IdentityUser> userManager)
        {
            _movieListService = movieListService;
            _userManager = userManager;
        }

        public MovieList MovieList { get; private set; }

        [BindProperty]
        public List<int> SelectedMovieIds { get; set; } = new();

        public List<SelectListItem> MovieOptions { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync(int listId)
        {
            MovieList = await _movieListService.GetMovieListByIdAsync(listId);

            if (MovieList == null)
            {
                return RedirectToPage("/Error");
            }

            MovieOptions = await _movieListService.GetAvailableMoviesForListAsync(listId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int listId)
        {
            await _movieListService.AddMoviesToListAsync(listId, SelectedMovieIds);

            return RedirectToPage("/Movies/MovieListDetails", new { id = listId });
        }
    }
}