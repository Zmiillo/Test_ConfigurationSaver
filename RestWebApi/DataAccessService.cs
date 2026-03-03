using CS.DataLibrary;
using CS.DataLibrary.Repositories;
using CS.LogicLayerLibrary;
using CS.LogicLayerLibrary.Interfaces;
using CS.LogicLayerLibrary.Services;
using CS.MessageHub.Interfaces;
using CS.MessageHub.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RestWebApi
{
    public static class DataAccessService
    {
        public sealed class ModuleConfiguration
        {
            public IServiceCollection Services { get; init; }
        }

        public static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            Action<ModuleConfiguration> action
        )
        {
            var moduleConfiguration = new ModuleConfiguration { Services = services };
            action(moduleConfiguration);
            return services;
        }

        public static void GetConnection(this ModuleConfiguration moduleConfiguration, string connectionString)
        {
            moduleConfiguration.Services.AddDbContext<CSContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            moduleConfiguration.Services.AddScoped(typeof(IRepository<>), typeof(RepositoriesGenerator<>));
            moduleConfiguration.Services.AddScoped<IMainService, MainService>();
            moduleConfiguration.Services.AddScoped<IDictionaryService, DictionaryService>();
            moduleConfiguration.Services.AddScoped<IMessageService, MessageService>();
        }
    }
}