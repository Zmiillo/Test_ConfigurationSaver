
using CS.DataLibrary.Models;
using CS.DataLibrary.Models.Request;
using CS.LogicLayerLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CS.RestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("Main")]
    [ApiController]
    public class ConfigurationSaverController : ControllerBase
    {
        private readonly IMainService _mainService;

        public ConfigurationSaverController(IMainService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// Создать конфигурацию
        /// </summary>
        /// <returns>int</returns>
        [HttpPost("CreateConfiguration")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateConfiguration(AddConfigurationRequestModel configuration)
        {
            var result = await _mainService.CreateConfigurationAsync(configuration);
            if (result > 0) return Ok(result);
            return BadRequest("ошибка создания");
        }

        /// <summary>
        /// Изменить конфигурацию
        /// </summary>
        /// <returns>int</returns>
        [HttpPost("UpdateConfiguration")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateConfiguration(EditConfigurationRequestModel configuration)
        {
            var result = await _mainService.UpdateConfigurationAsync(configuration);
            if (result > 0) return Ok(result);
            return BadRequest("ошибка правки");
        }

        /// <summary>
        /// Удалить конфигурацию
        /// </summary>
        [HttpPost("DeleteConfiguration")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteConfiguration(int configurationId)
        {
            var result = await _mainService.DeleteConfigurationAsync(configurationId);
            if (result) return Ok(result);
            return BadRequest("ошибка удаления");
        }

        /// <summary>
        ///  Получить список конфигураций
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [HttpGet("GetAllConfigurations")]
        [ProducesResponseType(typeof(List<CSUserConfiguration>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllConfigurations(int userId)
        {
            return Ok(await _mainService.GetAllConfigurationsAsync(userId));
        }

        /// <summary>
        ///  Получить последнюю конфигурацию
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [HttpGet("GetCurrentConfiguration")]
        [ProducesResponseType(typeof(CSUserConfiguration), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentConfiguration(int userId)
        {
            return Ok(await _mainService.GetCurrentConfigurationAsync(userId));
        }
    }
}
