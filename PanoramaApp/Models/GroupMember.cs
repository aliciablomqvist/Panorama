// <copyright file="GroupMember.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Models
{
    using Microsoft.AspNetCore.Identity;

    public class GroupMember
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}