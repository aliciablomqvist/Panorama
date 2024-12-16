// <copyright file="IGroupChatService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PanoramaApp.Models;

    public interface IGroupChatService
    {
        Task SendMessageAsync(string messageText, string userId, string userName, int groupId);

        Task<List<ChatMessage>> GetMessagesForGroupAsync(int groupId);
    }
}