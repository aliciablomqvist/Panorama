// <copyright file="GroupInvitation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Model for invitations to groups.
    /// </summary>
    public class GroupInvitation
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public string InvitedUserId { get; set; } = string.Empty;

        public string InvitedByUserId { get; set; } = string.Empty;

        public bool IsAccepted { get; set; }

        public DateTime InvitationDate { get; set; }
    }
}
