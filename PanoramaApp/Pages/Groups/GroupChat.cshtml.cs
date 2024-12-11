using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanoramaApp.Models;
using PanoramaApp.Data;
using PanoramaApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Pages.Groups
{
    public class GroupChatModel : PageModel
    {
        private readonly GroupChatService _chatService;
        private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
        public GroupChatModel(ApplicationDbContext context,GroupChatService chatService, UserManager<IdentityUser> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
            _context = context;
        }

        public List<ChatMessage> Messages { get; set; } = new();

        [BindProperty]
        public string MessageText { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

public async Task<IActionResult> OnGetAsync(int groupId)
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    // Check if the user is a member of the group
    var isMember = await _context.GroupMembers
        .AnyAsync(m => m.GroupId == groupId && m.UserId == user.Id);

    if (!isMember)
    {
        return Forbid(); // Restrict access if the user is not a member of the group
    }

    GroupId = groupId;
    Messages = await _chatService.GetMessagesForGroupAsync(groupId);

    return Page();
}

public async Task<IActionResult> OnPostAsync()
{
    var user = await _userManager.GetUserAsync(User);

    if (user == null)
    {
        return Unauthorized();
    }

    if (string.IsNullOrWhiteSpace(MessageText))
    {
        ModelState.AddModelError("MessageText", "Message cannot be empty.");
        return Page();
    }

    await _chatService.SendMessageAsync(MessageText, user.Id, user.UserName, GroupId);

    // Redirect back to the same page to update the chat history.
    return RedirectToPage(new { GroupId });
}
    }
}
