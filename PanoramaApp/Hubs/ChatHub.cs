// <copyright file="ChatHub.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
    public class ChatHub : Hub
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SendMessage(string user, string message)
        {
            await this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// Joins the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task JoinGroup(int groupId)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId.ToString());
        }

        /// <summary>
        /// Leaves the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LeaveGroup(int groupId)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupId.ToString());
        }
    }
}
