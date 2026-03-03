using Microsoft.AspNetCore.SignalR;
using CS.DataLibrary.Models;
using CS.DataLibrary.Repositories;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using MessageHub.Models;
using CS.MessageHub.Interfaces;

namespace CS.MessageHub.Services
{
    public class MessageService : Hub, IMessageService
    {
        private readonly IRepository<CSMessage> _csMessageRepository;
        private readonly IRepository<CSUser> _csUserRepository;
        private readonly List<UserConnection> userConnections = new();
        private const int missedMessagesCount = 5;
        private const string systemLogin = "UpdateInformationBot";

        public MessageService(IRepository<CSMessage> csMessageRepository, IRepository<CSUser> csUserRepository)
        {
            _csMessageRepository = csMessageRepository;
            _csUserRepository = csUserRepository;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceiveSystemMessage", "Connected to server!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (userConnections.Select(x => x.ConnectionId).Contains(Context.ConnectionId))
            {
                int index = userConnections.FindIndex(i => i.ConnectionId == Context.ConnectionId);
                userConnections.RemoveAt(index);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<string> JoinServiceAsync(int userId)
        {
            try
            {
                var user = await _csUserRepository.GetFirstOrDefaultAsync(x => x.Id == userId);
                if (user is not null)
                {
                    var userConnection = new UserConnection()
                    {
                        ConnectionId = Context.ConnectionId,
                        Login = user.Login
                    };

                    if (!string.IsNullOrEmpty(user.Subscribes))
                    {
                        var subscribes = user.Subscribes.Split().Select(int.Parse).ToList();
                        var recentMessages = await _csMessageRepository
                            .GetByFilterAsync(x => subscribes.Contains(x.SubscribeId));
                        if (recentMessages.Any())
                        {
                            await Clients.Caller.SendAsync("LoadRecentMessages", recentMessages.OrderByDescending(x => x.Id).Take(missedMessagesCount));
                        }
                        userConnection.Subscribes = subscribes;
                    }

                    userConnections.Add(userConnection);
                    return Context.ConnectionId;
                }
            }
            catch
            { }
            return string.Empty;
        }

        public async Task<int> SendMessageAsync(CSMessage message)
        {
            try
            {
                var users = userConnections.Where(x => x.Subscribes != null && x.Subscribes.Any() && x.Subscribes.Contains(message.SubscribeId));
                if (users.Any())
                {
                    var connections = users.Select(x => x.ConnectionId).ToList();
                    await Clients.Clients(connections).SendAsync("ReceiveMessage", systemLogin, message.Content, DateTime.Now);
                }

                // сохраняем сообщение в базу, чтобы его потом могли прочитать подписчики
                return await _csMessageRepository.AddAsync(message);
            }
            catch(Exception ex)
            { }

            return default;
        }

        public async Task<List<string>> GetOnlineUsersAsync()
        {
            var onlineUsers = userConnections.Select(x => x.Login).ToList();
            await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
            return onlineUsers;
        }
    }
}

