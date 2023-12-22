using Application.Storage;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AzureStorage.TableStorageService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRequestLogTable(this IServiceCollection services)
        {
            services.AddSingleton<TableClient>(s =>
            {
                var options = s.GetService<IOptions<TableStorageSettings>>();
                if (options?.Value == null)
                {
                    throw new ArgumentNullException(nameof(TableStorageSettings));
                }
                
                var tableServiceClient = new TableServiceClient(options.Value.StorageConnectionString);
                CreateTableIfNotExists(tableServiceClient, options.Value.LogTableName).GetAwaiter().GetResult();
                return tableServiceClient.GetTableClient(options.Value.LogTableName);
            });

            services.AddSingleton<IRequestAttemptStorage, TableStorageService>();
            return services;
        }
        
        private static async Task CreateTableIfNotExists(TableServiceClient tableServiceClient, string tableName)
        {
            await tableServiceClient.CreateTableIfNotExistsAsync(tableName);
        }
    }
}