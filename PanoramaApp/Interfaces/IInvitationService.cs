// <copyright file="IInvitationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    public interface IInvitationService
    {
        Task<List<GroupInvitation>> GetPendingInvitationsAsync(string userId);

        Task AcceptInvitationAsync(int invitationId, string userId);

        Task SendInvitationAsync(int groupId, string invitedUserId, string invitedByUserId);
    }
}
