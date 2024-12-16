// <copyright file="GroupChatContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Model for the context for group chats
    /// </summary>
    public class GroupChatContext
    {
        public int Id { get; set; }

        public string GroupName { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }
    }
}
