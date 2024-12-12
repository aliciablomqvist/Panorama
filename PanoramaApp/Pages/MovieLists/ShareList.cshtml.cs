// <copyright file="ShareList.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.MovieLists
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class ShareModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public ShareModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public MovieList MovieList { get; set; }

        public string ShareableLink { get; set; }

        public int ListId { get; set; }

        public string RecipientUserName { get; set; }

        public async Task<IActionResult> OnGetAsync(int listId)
        {
            this.MovieList = await this.context.MovieLists.FindAsync(listId);

            if (this.MovieList == null)
            {
                return this.NotFound();
            }

            // Delbar länk
            this.ShareableLink = this.Url.Page("/Movies/MovieListDetails", null, new { id = listId }, this.Request.Scheme);

            return this.Page();
        }
    }
}