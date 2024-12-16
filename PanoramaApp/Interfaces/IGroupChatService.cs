using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IGroupChatService
    {
        Task SendMessageAsync(string messageText, string userId, string userName, int groupId);
        Task<List<ChatMessage>> GetMessagesForGroupAsync(int groupId);
    }
}
