using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Interfaces;
using PanoramaApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly ApplicationDbContext _context;

        public InvitationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GroupInvitation>> GetPendingInvitationsAsync(string userId)
        {
            return await _context.GroupInvitations
                .Where(i => i.InvitedUserId == userId && !i.IsAccepted)
                .ToListAsync();
        }

        public async Task AcceptInvitationAsync(int invitationId, string userId)
        {
            var invitation = await _context.GroupInvitations
                .Include(i => i.Group)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null || invitation.InvitedUserId != userId)
            {
                throw new InvalidOperationException("Invalid invitation or user.");
            }

            invitation.IsAccepted = true;
            _context.GroupInvitations.Update(invitation);

            var isAlreadyMember = await _context.GroupMembers
                .AnyAsync(m => m.GroupId == invitation.GroupId && m.UserId == userId);

            if (!isAlreadyMember)
            {
                var newMember = new GroupMember
                {
                    GroupId = invitation.GroupId,
                    UserId = userId,
                };

                _context.GroupMembers.Add(newMember);
            }

            await _context.SaveChangesAsync();
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

            _context.GroupInvitations.Add(newInvitation);
            await _context.SaveChangesAsync();
        }
    }
}
