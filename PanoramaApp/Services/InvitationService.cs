// <copyright file="InvitationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class InvitationService : IInvitationService
    {
        private readonly ApplicationDbContext context;

        public InvitationService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<GroupInvitation>> GetPendingInvitationsAsync(string userId)
        {
            return await this.context.GroupInvitations
                .Where(i => i.InvitedUserId == userId && !i.IsAccepted)
                .ToListAsync();
        }

        public async Task AcceptInvitationAsync(int invitationId, string userId)
        {
            var invitation = await this.context.GroupInvitations
                .Include(i => i.Group)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null || invitation.InvitedUserId != userId)
            {
                throw new InvalidOperationException("Invalid invitation or user.");
            }

            invitation.IsAccepted = true;
            this.context.GroupInvitations.Update(invitation);

            var isAlreadyMember = await this.context.GroupMembers
                .AnyAsync(m => m.GroupId == invitation.GroupId && m.UserId == userId);

            if (!isAlreadyMember)
            {
                var newMember = new GroupMember
                {
                    GroupId = invitation.GroupId,
                    UserId = userId,
                };

                this.context.GroupMembers.Add(newMember);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task SendInvitationAsync(int groupId, string invitedUserId, string invitedByUserId)
        {
            var newInvitation = new GroupInvitation
            {
                GroupId = groupId,
                InvitedUserId = invitedUserId,
                InvitedByUserId = invitedByUserId,
                IsAccepted = false,
                InvitationDate = DateTime.UtcNow,
            };

            this.context.GroupInvitations.Add(newInvitation);
            await this.context.SaveChangesAsync();
        }
    }
}
