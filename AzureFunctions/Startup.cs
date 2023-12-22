using System;
using Application.Configuration;
using AzureFunctionApplication;
using AzureFunctionApplication.Configuration.Settings;
using AzureStorage.BlobStorageService;
using AzureStorage.TableStorageService;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomApiRequest.Configuration;


[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctionApplication
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            
            
            LoadAppSettings(builder);
            builder.Services.AddBlobStorageContainer();
            builder.Services.AddRequestLogTable();
            builder.Services.AddApplication();
            builder.Services.AddRandomApiRequests();
        }
        
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);
            builder.ConfigurationBuilder
                .AddJsonFile(ConfigFilePath, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
        
        private static void LoadAppSettings(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ApplicationSettings>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection(AzureAppSettingsKey).Bind(settings); });

            builder.Services.AddOptions<BlobStorageSettings>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection(StorageSettingsKey).Bind(settings); });

            builder.Services.AddOptions<TableStorageSettings>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection(StorageSettingsKey).Bind(settings); });

            builder.Services.AddOptions<RandomApiSettings>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection(nameof(RandomApiSettings)).Bind(settings); });
        }

        #region Constants
        private const string AzureAppSettingsKey = "AzureFunctionApplicationSettings";
        private const string StorageSettingsKey = "StorageSettings";
        private const string ConfigFilePath = "appsettings.json";
        #endregion
    }
}