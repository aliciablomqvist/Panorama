// <copyright file="VoteFilms.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Threading.Tasks;

namespace PanoramaApp.Pages.Groups
{
    public class VoteFilmsModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly IVoteService _voteService;
        private readonly UserManager<IdentityUser> _userManager;

        public VoteFilmsModel(IGroupService groupService, IVoteService voteService, UserManager<IdentityUser> userManager)
        {
            _groupService = groupService;
            _voteService = voteService;
            _userManager = userManager;
        }

        public Group Group { get; private set; }
        public int GroupId { get; private set; }

        public async Task<IActionResult> OnGetAsync(int groupId)
        {
            Group = await _groupService.GetGroupByIdAsync(groupId);

            if (Group == null)
            {
                return RedirectToPage("/Error");
            }

            GroupId = groupId;
            return Page();
        }

        public async Task<IActionResult> OnPostVoteAsync(int groupId, int movieId)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            await _voteService.AddVoteAsync(groupId, movieId, userId);

            return RedirectToPage(new { groupId });
        }

        public async Task<int> GetVotesForMovieAsync(int movieId)
        {
            return await _voteService.GetVotesForMovieAsync(movieId);
        }
    }
}
