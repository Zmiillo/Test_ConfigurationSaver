using CS.DataLibrary.Models;
using CS.DataLibrary.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.LogicLayerLibrary.Interfaces
{
    public interface IMainService
    {
        Task<int> CreateConfigurationAsync(AddConfigurationRequestModel configuration);

        Task<int> UpdateConfigurationAsync(EditConfigurationRequestModel configuration);

        Task<bool> DeleteConfigurationAsync(int configurationId);

        /// <summary>
        /// Получает последнюю конфигурацию по пользователю
        /// </summary>
        Task<CSUserConfiguration> GetCurrentConfigurationAsync(int userId);

        Task<IEnumerable<CSUserConfiguration>> GetAllConfigurationsAsync(int userId);
    }
}
