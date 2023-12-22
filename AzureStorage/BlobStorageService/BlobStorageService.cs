using System.Text;
using Application.Storage;
using Azure.Storage.Blobs;
using Domain;

namespace AzureStorage.BlobStorageService
{
    public class BlobStorageService : IContentStorage
    {

        public BlobStorageService(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }
        
        public async Task<MemoryStream> GetContentStreamAsync(string id, CancellationToken cancellationToken)
        {
            var blobClient = CreateBlobClientFor(id);
            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream, cancellationToken);
            return stream;
        }

        public async Task UploadRequestContentAsync(RequestAttemptResult requestAttemptResult, bool shouldOverwrite)
        {
            var blobClient = CreateBlobClientFor(requestAttemptResult.Id.ToString());
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestAttemptResult.Content));
            await blobClient.UploadAsync(stream, overwrite: shouldOverwrite);
        }

        public async Task<bool> BlobExists(string id, CancellationToken cancellationToken)
        {
            var blobClient = CreateBlobClientFor(id);
            return await blobClient.ExistsAsync(cancellationToken);
        }

        /// <summary>
        /// Creates the blob client for specific blob.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        private BlobClient CreateBlobClientFor(string id)
        {
            var blobName = CreateBlobNameFromId(id);
            return _blobContainerClient.GetBlobClient(blobName);
        }
        
        private string CreateBlobNameFromId(string id) => $"{id}.json";

        #region Fields
        private readonly BlobContainerClient _blobContainerClient;
        #endregion
    }
}