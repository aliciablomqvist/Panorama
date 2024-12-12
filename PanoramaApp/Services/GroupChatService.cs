// <copyright file="GroupChatService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PanoramaApp.Data;
    using PanoramaApp.Models;

    public class GroupChatService
    {
        private readonly ApplicationDbContext context;

        public GroupChatService(ApplicationDbContext context)
        {
            this.context = context;
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

            this.context.ChatMessages.Add(chatMessage);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesForGroupAsync(int groupId)
        {
            return await this.context.ChatMessages
                .Where(m => m.GroupId == groupId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
}