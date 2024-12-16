// <copyright file="GroupChatService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class GroupChatService : IGroupChatService
    {
        private readonly ApplicationDbContext context;

        /// <summary>Initializes a new instance of the <see cref="GroupChatService" /> class.</summary>
        /// <param name="context">The context.</param>
        public GroupChatService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="messageText">The message text.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Gets the messages for group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<List<ChatMessage>> GetMessagesForGroupAsync(int groupId)
        {
            return await this.context.ChatMessages
                .Where(m => m.GroupId == groupId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
}
