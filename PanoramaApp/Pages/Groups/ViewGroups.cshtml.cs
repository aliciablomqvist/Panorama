// <copyright file="ViewGroups.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Pages.Groups
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class ViewGroupsModel : PageModel
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGroupsModel"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ViewGroupsModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        public List<Group> Groups { get; set; } = new List<Group>();

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnGetAsync()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Filtrera grupper där användaren är medlem
            this.Groups = await this.context.Groups
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .Include(g => g.Movies)
                .Where(g => g.Members.Any(m => m.UserId == userId)) // Visa endast grupper där användaren är medlem
                .ToListAsync();
        }
    }
}
