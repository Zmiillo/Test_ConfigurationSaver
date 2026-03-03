using CS.DataLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.MessageHub.Interfaces
{
    public interface IMessageService
    {
        Task<string> JoinServiceAsync(int userId);

        Task<int> SendMessageAsync(CSMessage message);

        Task<List<string>> GetOnlineUsersAsync();
    }
}
