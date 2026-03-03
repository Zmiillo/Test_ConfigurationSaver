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

namespace CS.UnitTests
{
    [TestClass]
    public class MainServiceTests
    {
        private readonly int userId = 1;
        private readonly int id = 1;

        [TestMethod]
        public async Task CreateConfigurationAsync_WhenSetConfigurationWithUser_ThenIdReceived()
        {
            var addConfigurationRequestModel = new AddConfigurationRequestModel()
            {
                UserId = userId
            };

            var configurationMock = new Mock<IRepository<CSUserConfiguration>>();
            configurationMock.Setup(m => m.AddAsync(It.IsAny<CSUserConfiguration>())).ReturnsAsync(id);

            var mainService = new MainService(configurationMock.Object);

            var result = await mainService.CreateConfigurationAsync(addConfigurationRequestModel);

            result.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task UpdateConfigurationAsync_WhenSetConfigurationWithUser_ThenIdReceived()
        {
            var editConfigurationRequestModel = new EditConfigurationRequestModel()
            {
                Id = id
            };

            var configurationMock = new Mock<IRepository<CSUserConfiguration>>();
            configurationMock.Setup(m => m.UpdateAsync(It.IsAny<CSUserConfiguration>())).ReturnsAsync(id);

            var mainService = new MainService(configurationMock.Object);

            var result = await mainService.UpdateConfigurationAsync(editConfigurationRequestModel);

            result.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task DeleteConfigurationAsync_WhenGetId_ThenConfigurationDeleted()
        {
            var configurationMock = new Mock<IRepository<CSUserConfiguration>>();
            configurationMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            var mainService = new MainService(configurationMock.Object);

            var result = await mainService.DeleteConfigurationAsync(It.IsAny<int>());

            result.Should().Be(true);
        }

        [TestMethod]
        public async Task GetConfigurationAsync_WhenIdUserRequest_ThenConfigurationReceived()
        {
            var csUserConfigurations = new[]
            {
                new CSUserConfiguration() { Id = 1, UserId = userId, Width = 100 },
                new CSUserConfiguration() { Id = 3, UserId = userId, Width = 200 },
                new CSUserConfiguration() { Id = 2, UserId = userId, Width = 300 },
            };

            var configurationMock = new Mock<IRepository<CSUserConfiguration>>();
            configurationMock
                .Setup(m => m.GetByFilterAsync(It.IsAny<Expression<Func<CSUserConfiguration, bool>>>()))
                .ReturnsAsync(csUserConfigurations);

            var mainService = new MainService(configurationMock.Object);

            var result = await mainService.GetCurrentConfigurationAsync(userId);

            result.Width.Should().Be(200);
        }
    }
}

