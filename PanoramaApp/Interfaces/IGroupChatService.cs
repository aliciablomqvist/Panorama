// <copyright file="IGroupChatService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for group chat operations
    /// </summary>
    public interface IGroupChatService
    {
        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="messageText">The message text.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task SendMessageAsync(string messageText, string userId, string userName, int groupId);

        /// <summary>
        /// Gets the messages for group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<List<ChatMessage>> GetMessagesForGroupAsync(int groupId);
    }
}
