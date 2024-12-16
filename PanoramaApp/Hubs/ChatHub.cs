// <copyright file="ChatHub.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await this.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinGroup(int groupId)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId.ToString());
        }

        public async Task LeaveGroup(int groupId)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupId.ToString());
        }
    }
}
