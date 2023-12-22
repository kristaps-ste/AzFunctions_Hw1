using Domain;

namespace Application.Storage
{
    public interface IContentStorage
    {
        /// <summary>
        /// Returns the content of the blob as a stream.
        /// </summary>
        /// <param name="blobName">The blob name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Stream of content.</returns>
        Task<MemoryStream> GetContentStreamAsync(string blobName, CancellationToken cancellationToken);

        /// <summary>
        /// Uploads the content to the specified blob.
        /// </summary>
        /// <param name="requestAttemptResult">Request attempt.</param>
        /// <param name="shouldOverwrite">Allows overwrite if blob exists.</param>
        Task UploadRequestContentAsync(RequestAttemptResult requestAttemptResult,  bool shouldOverwrite);

        /// <summary>
        /// Checks if blob exists.
        /// </summary>
        /// <param name="blobName">The blob name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if blob exists</returns>
        Task<bool> BlobExists(string blobName, CancellationToken cancellationToken);
    }
}