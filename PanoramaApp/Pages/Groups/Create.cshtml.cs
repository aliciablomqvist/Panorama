using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CreateGroupModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateGroupModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    public List<IdentityUser> Users { get; set; } = new List<IdentityUser>();

    [BindProperty]
    public List<string> SelectedUsers { get; set; } = new List<string>();

    public async Task OnGetAsync()
    {
   
        Users = await _context.Users.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    var currentUser = await _userManager.GetUserAsync(User); // Get the current logged-in user
    if (currentUser == null)
    {
        // Handle the case where the user is not logged in (redirect to login page or show an error)
        return RedirectToPage("/Account/Login");
    }

    var newGroup = new Group
    {
        Name = Name,
        OwnerId = currentUser.Id // Set the OwnerId to the current user's ID
    };

    _context.Groups.Add(newGroup);
    await _context.SaveChangesAsync();

    foreach (var userId in SelectedUsers)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var groupMember = new GroupMember
            {
                GroupId = newGroup.Id,
                UserId = user.Id
            };
            _context.GroupMembers.Add(groupMember);
        }
    }

    await _context.SaveChangesAsync();

    return RedirectToPage("/Groups/ViewGroups");
}
}