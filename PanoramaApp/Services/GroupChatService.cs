using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaApp.Services
{
    public class GroupChatService : IGroupChatService
    {
        private readonly ApplicationDbContext _context;

        public GroupChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessageAsync(string messageText, string userId, string userName, int groupId)
        {
            var chatMessage = new ChatMessage
            {
                MessageText = messageText,
                UserId = userId,
                UserName = userName,
                Timestamp = DateTime.UtcNow,
                GroupId = groupId,
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesForGroupAsync(int groupId)
        {
            return await _context.ChatMessages
                .Where(m => m.GroupId == groupId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
}
