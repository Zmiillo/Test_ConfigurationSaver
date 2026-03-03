using CS.DataLibrary.Models;
using CS.DataLibrary.Models.Request;
using CS.DataLibrary.Repositories;
using CS.LogicLayerLibrary.Services;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System;
using CS.MessageHub.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Collections.Generic;

namespace CS.UnitTests
{
    [TestClass]
    public class MessageServiceTests
    {
        private readonly int testId = 1;
        private readonly string testString = "test";

        [TestMethod]
        public async Task JoinServiceAsync_WhenError_ThenStringEmptyReceived()
        {
            var messageMock = new Mock<IRepository<CSMessage>>();
            var userMock = new Mock<IRepository<CSUser>>();
            var messageService = new MessageService(messageMock.Object, userMock.Object);

            var result = await messageService.JoinServiceAsync(testId);

            result.Should().Be(string.Empty);
        }


        [TestMethod]
        public async Task JoinServiceAsync_WhenUserAdded_ThenConnectionIdReceived()
        {
            var user = new CSUser();

            var messageMock = new Mock<IRepository<CSMessage>>();
            var userMock = new Mock<IRepository<CSUser>>();
            var clientsMock = new Mock<IHubCallerClients>();
            var clientProxyMock = new Mock<IClientProxy>();
            var contextMock = new Mock<HubCallerContext>();

            clientsMock.Setup(clients => clients.All).Returns(clientProxyMock.Object);
            userMock.Setup(s => s.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CSUser, bool>>>())).ReturnsAsync(user);
            contextMock.Setup(c => c.ConnectionId).Returns(testString);

            var messageService = new MessageService(messageMock.Object, userMock.Object)
            {
                Clients = clientsMock.Object,
                Context = contextMock.Object
            };

            var result = await messageService.JoinServiceAsync(testId);

            result.Should().Be(testString);
        }

        [TestMethod]
        public async Task SendMessageAsync_WhenUserIsSubscribed_ThenMessageSended()
        {
            var user = new CSUser()
            {
                Login = testString,
                Subscribes = testId.ToString()
            };
            var message = new CSMessage() { SubscribeId = testId };
            var messages = new[]
            {
                message
            };

            var messageMock = new Mock<IRepository<CSMessage>>();
            var userMock = new Mock<IRepository<CSUser>>();
            var clientsMock = new Mock<IHubCallerClients>();
            var clientProxyMock = new Mock<IClientProxy>();
            var contextMock = new Mock<HubCallerContext>();

            clientsMock.Setup(clients => clients.Clients(It.IsAny<List<string>>())).Returns(clientProxyMock.Object);
            userMock.Setup(s => s.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CSUser, bool>>>())).ReturnsAsync(user);
            contextMock.Setup(c => c.ConnectionId).Returns(testString);
            messageMock.Setup(s => s.AddAsync(It.IsAny<CSMessage>())).ReturnsAsync(testId);

            var messageService = new MessageService(messageMock.Object, userMock.Object)
            {
                Clients = clientsMock.Object,
                Context = contextMock.Object
            };

            var addUserConnection = await messageService.JoinServiceAsync(testId);

            var result = await messageService.SendMessageAsync(message);

            result.Should().Be(testId);
        }

        [TestMethod]
        public async Task GetOnlineUsersAsync_WhenOneUserOnline_ThenOneReceived()
        {
            var user = new CSUser()
            {
                Login = testString,
                Subscribes = testId.ToString()
            };
            var messages = new[]
            {
                new CSMessage() { SubscribeId = testId }
            };

            var messageMock = new Mock<IRepository<CSMessage>>();
            var userMock = new Mock<IRepository<CSUser>>();
            var clientsMock = new Mock<IHubCallerClients>();
            var clientProxyMock = new Mock<IClientProxy>();
            var contextMock = new Mock<HubCallerContext>();

            clientsMock.Setup(clients => clients.Caller).Returns(clientProxyMock.Object);
            userMock.Setup(s => s.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CSUser, bool>>>())).ReturnsAsync(user);
            messageMock.Setup(s => s.GetByFilterAsync(It.IsAny<Expression<Func<CSMessage, bool>>>())).ReturnsAsync(messages);
            contextMock.Setup(c => c.ConnectionId).Returns(testString);

            var messageService = new MessageService(messageMock.Object, userMock.Object)
            {
                Clients = clientsMock.Object,
                Context = contextMock.Object
            };

            var addUserConnection = await messageService.JoinServiceAsync(testId);

            var result = await messageService.GetOnlineUsersAsync();

            result.Count.Should().Be(1);

            clientProxyMock.Verify(c => c.SendCoreAsync("OnlineUsers",
                    It.Is<object[]>(o => o != null && o.Length == 1),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}

