using PanoramaApp.Data;
using PanoramaApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Services
{
    public class GroupChatService
    {
        private readonly ApplicationDbContext _context;

        public GroupChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessageAsync(string messageText, string userName, int groupId)
        {
            var chatMessage = new ChatMessage
            {
                MessageText = messageText,
                UserName = userName,
                Timestamp = DateTime.UtcNow,
                GroupId = groupId
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
