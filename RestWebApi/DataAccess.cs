using System;

using Microsoft.Extensions.DependencyInjection;
using static RestWebApi.DataAccessService;

namespace RestWebApi
{
    public static class DataAccessModuleWebApi
    {
        public static IServiceCollection AddDataAccessModule(
               this IServiceCollection services,
               Action<ModuleConfiguration> action
           )
        {
            return DataAccessService.AddDataAccess(services, action);
        }

        public static void InPostgress(this ModuleConfiguration moduleConfiguration, string connectionString)
        {
            DataAccessService.GetConnection(moduleConfiguration, connectionString);
        }
    }
}
