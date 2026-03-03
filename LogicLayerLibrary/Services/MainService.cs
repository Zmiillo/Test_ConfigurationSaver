using CS.DataLibrary.Models;
using CS.DataLibrary.Models.Request;
using CS.DataLibrary.Repositories;
using CS.LogicLayerLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.LogicLayerLibrary.Services
{
    public class MainService : IMainService
    { 
        private readonly IRepository<CSUserConfiguration> _csUserConfigurationRepository;

        private const int errorCode = -1;
        private const int maxUserConfigurations = 3;

        public MainService(IRepository<CSUserConfiguration> csUserConfigurationRepository)
        {
            _csUserConfigurationRepository = csUserConfigurationRepository;
        }

        public async Task<int> CreateConfigurationAsync(AddConfigurationRequestModel configuration)
        {
            try
            {
                var getConfigurations = await _csUserConfigurationRepository.GetByFilterAsync(x => x.UserId == configuration.UserId && !x.IsDeleted);
                if (getConfigurations.Count() < maxUserConfigurations)
                {
                    var mappingConfiguration = new CSUserConfiguration(configuration);
                    return await _csUserConfigurationRepository.AddAsync(mappingConfiguration);
                }
                return default;
            }
            catch
            {
            }
            return errorCode;
        }

        public async Task<int> UpdateConfigurationAsync(EditConfigurationRequestModel configuration)
        {
            try
            {
                var getConfiguration = await _csUserConfigurationRepository.GetFirstOrDefaultAsync(x => x.Id == configuration.Id);
                if (getConfiguration is not null)
                {
                    var mappingConfiguration = new CSUserConfiguration(configuration);
                    mappingConfiguration.UserId = getConfiguration.UserId;
                    return await _csUserConfigurationRepository.UpdateAsync(mappingConfiguration);
                }
            }
            catch
            {
            }
            return errorCode;
        }

        public async Task<bool> DeleteConfigurationAsync(int configurationId)
        {
            try
            {
                return await _csUserConfigurationRepository.RemoveAsync(configurationId);
            }
            catch
            {
            }
            return false;
        }

        public async Task<CSUserConfiguration> GetCurrentConfigurationAsync(int userId)
        {
            try
            {
                var result = await GetAllConfigurationsAsync(userId);
                if (result.Any())
                {
                    return result.FirstOrDefault();
                }
            }
            catch
            {
            }
            return null;
        }

        public async Task<IEnumerable<CSUserConfiguration>> GetAllConfigurationsAsync(int userId)
        {
            try
            {
                var result = await _csUserConfigurationRepository.GetByFilterAsync(x => x.UserId == userId && !x.IsDeleted);
                if (result.Any())
                {
                    return result.OrderByDescending(x => x.Id);
                }
            }
            catch
            {
            }
            return Enumerable.Empty<CSUserConfiguration>();
        }
    }
}

