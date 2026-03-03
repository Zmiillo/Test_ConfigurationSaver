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
using CS.DataLibrary.Models.Dictionaries;
using CS.MessageHub.Services;
using CS.MessageHub.Interfaces;

namespace CS.UnitTests
{
    [TestClass]
    public class DictionaryServiceTests
    {
        private readonly int id = 1;

        [TestMethod]
        public async Task AddRecordAsync_WhenRecordAdded_ThenIdReceived()
        {
            var font = new CSFont();
            var color = new CSColor();
            var subscribe = new CSSubscribeType();

            var fontMock = new Mock<IRepository<CSFont>>();
            var colorMock = new Mock<IRepository<CSColor>>();
            var subscribeMock = new Mock<IRepository<CSSubscribeType>>();
            var messageServiceMock = new Mock<IMessageService>();

            fontMock.Setup(m => m.AddAsync(It.IsAny<CSFont>())).ReturnsAsync(id);
            colorMock.Setup(m => m.AddAsync(It.IsAny<CSColor>())).ReturnsAsync(id);
            subscribeMock.Setup(s => s.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CSSubscribeType, bool>>>())).ReturnsAsync(subscribe);
            messageServiceMock.Setup(s => s.SendMessageAsync(It.IsAny<CSMessage>())).ReturnsAsync(id);

            var dictionaryService = new DictionaryService(fontMock.Object, colorMock.Object, subscribeMock.Object, messageServiceMock.Object);

            var resultFont = await dictionaryService.AddRecordAsync(font);
            var resultColor = await dictionaryService.AddRecordAsync(color);

            resultFont.Should().Be(id);
            resultColor.Should().Be(id);
        }

        [TestMethod]
        public async Task EditRecordAsync_WhenRecordModified_ThenIdReceived()
        {
            var font = new CSFont();
            var color = new CSColor();
            var subscribe = new CSSubscribeType();

            var fontMock = new Mock<IRepository<CSFont>>();
            var colorMock = new Mock<IRepository<CSColor>>();
            var subscribeMock = new Mock<IRepository<CSSubscribeType>>();
            var messageServiceMock = new Mock<IMessageService>();

            fontMock.Setup(m => m.UpdateAsync(It.IsAny<CSFont>())).ReturnsAsync(id);
            colorMock.Setup(m => m.UpdateAsync(It.IsAny<CSColor>())).ReturnsAsync(id);
            subscribeMock.Setup(s => s.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CSSubscribeType, bool>>>())).ReturnsAsync(subscribe);
            messageServiceMock.Setup(s => s.SendMessageAsync(It.IsAny<CSMessage>())).ReturnsAsync(id);

            var dictionaryService = new DictionaryService(fontMock.Object, colorMock.Object, subscribeMock.Object, messageServiceMock.Object);

            var resultFont = await dictionaryService.EditRecordAsync(font);
            var resultColor = await dictionaryService.EditRecordAsync(color);

            resultFont.Should().Be(id);
            resultColor.Should().Be(id);
        }
    }
}

