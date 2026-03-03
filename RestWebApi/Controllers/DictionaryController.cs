
using CS.DataLibrary.Models;
using CS.DataLibrary.Models.Dictionaries;
using CS.DataLibrary.Models.Request;
using CS.LogicLayerLibrary;
using CS.LogicLayerLibrary.Interfaces;
using CS.MessageHub.Interfaces;
using DataLibrary.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CS.RestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("Dictionary")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        /// <summary>
        /// Создать запись и отправить подписчикам
        /// </summary>
        /// <returns>int</returns>
        [HttpPost("AddRecord")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddRecord(DictionaryRequestModel request)
        {
            var result = request.DictionaryName switch
            {
                "Font" => await _dictionaryService.AddRecordAsync(new CSFont(request)),
                "Color" => await _dictionaryService.AddRecordAsync(new CSColor(request)),
                _ => default
            };
            if (result > 0)
            {
                return Ok(result);
            }
            return BadRequest("ошибка создания");
        }

        /// <summary>
        /// Редактировать запись и отправить подписчикам
        /// </summary>
        /// <returns>int</returns>
        [HttpPost("EditRecord")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EditRecord(DictionaryRequestModel request)
        {
            var result = request.DictionaryName switch
            {
                "Font" => await _dictionaryService.EditRecordAsync(new CSFont(request)),
                "Color" => await _dictionaryService.EditRecordAsync(new CSColor(request)),
                _ => default
            };
            if (result > 0)
            {
                return Ok(result);
            }

            return BadRequest("ошибка редактирования");
        }       
    }
}
