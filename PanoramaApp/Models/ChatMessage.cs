// <copyright file="ChatMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Model for chat messages within groups.
    /// </summary>
    public class ChatMessage
    {
        public int Id { get; set; }

        public string MessageText { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
