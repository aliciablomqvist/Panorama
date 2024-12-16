// <copyright file="IInvitationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for invitations to groups
    /// </summary>
    public interface IInvitationService
    {
        /// <summary>
        /// Gets the pending invitations asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<GroupInvitation>> GetPendingInvitationsAsync(string userId);

        /// <summary>
        /// Accepts the invitation asynchronous.
        /// </summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task AcceptInvitationAsync(int invitationId, string userId);

        /// <summary>
        /// Sends the invitation asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="invitedUserId">The invited user identifier.</param>
        /// <param name="invitedByUserId">The invited by user identifier.</param>
        /// <returns></returns>
        Task SendInvitationAsync(int groupId, string invitedUserId, string invitedByUserId);
    }
}
