using Application.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AzureStorage.BlobStorageService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBlobStorageContainer(this IServiceCollection services)
        {
            services.AddSingleton<BlobContainerClient>((s) =>
            {
                var options = s.GetService<IOptions<BlobStorageSettings>>();
                var blobServiceClient = new BlobServiceClient(options?.Value.StorageConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(options?.Value.BlobContentContainerName);
                CreateContainerIfNotExists(containerClient).GetAwaiter().GetResult();
                return containerClient;
            });
            
            services.AddSingleton<IContentStorage, BlobStorageService>();
            return services;
        }

        private static async Task CreateContainerIfNotExists(BlobContainerClient containerClient)
        {
                await containerClient.CreateIfNotExistsAsync();
        }
    }
}