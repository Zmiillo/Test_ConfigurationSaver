
using CS.DataLibrary.Models;
using CS.DataLibrary.Models.Request;
using CS.LogicLayerLibrary;
using CS.MessageHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CS.RestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("Message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Подключиться к комнате
        /// </summary>
        [HttpGet("JoinMessageService")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> JoinMessageService(int userId)
        {
            var result = await _messageService.JoinServiceAsync(userId);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(result);
            }
            return BadRequest("Подключение не удалось");
        }

        /// <summary>
        /// Метод отправки сообщений по подписчикам
        /// </summary>
        /// <param name="message">Сообщение для отправки</param>
        /// <returns>int</returns>
        [HttpPost("SendMessage")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendMessage(CSMessage message)
        {
            var result = await _messageService.SendMessageAsync(message);
            if (result > 0)
            {
                return Ok(result);
            }
            return BadRequest("Сообщение не отправлено");
        }

        /// <summary>
        /// Получение текущего списка подключенных
        /// </summary>
        [HttpGet("GetOnlineUsers")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteConfiguration()
        {
            var result = await _messageService.GetOnlineUsersAsync();
            return Ok(result);
        }
    }
}
