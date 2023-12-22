using System.Threading;
using System.Threading.Tasks;
using Application.Storage;
using AzureFunctionApplication.ContentApi.CustomActionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;


namespace AzureFunctionApplication.ContentApi
{
    public class GetBlobContent
    {
        private readonly IContentStorage _contentStorage;

        public GetBlobContent(IContentStorage contentStorage)
        {
            _contentStorage = contentStorage;
        }

        [FunctionName(nameof(GetBlobContent))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPayload")] HttpRequest req,
            CancellationToken cancellationToken)
        {
            var id = req.Query[LogIdQueryParameterName];
            var blobExists = await _contentStorage.BlobExists(id, cancellationToken);
            if (!blobExists)
            {
                return new NotFoundResult();
            }

            var contentStream = await _contentStorage.GetContentStreamAsync(id, cancellationToken);
            contentStream.Position = 0;
            return new TextStreamActionResult(contentStream);
        }

        #region Constants
            private const string LogIdQueryParameterName = "logId";
        #endregion
    }
}