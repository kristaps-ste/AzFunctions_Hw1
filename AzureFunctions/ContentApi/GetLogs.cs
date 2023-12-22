using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Application.Storage;
using AzureFunctionApplication.Configuration.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AzureFunctionApplication.ContentApi
{
    public class GetLogs
    {
        public GetLogs(IRequestAttemptStorage requestAttemptStorage, IOptions<ApplicationSettings> settings)
        {
            _requestAttemptStorage = requestAttemptStorage;
            _settings = settings;
        }
        
        [FunctionName(nameof(GetLogs))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetLogs")] HttpRequest req, 
            CancellationToken cancellationToken)
        {
            string from = req.Query[FromQueryParameterName];
            string to = req.Query[ToQueryParameterName];
            try
            {
                var fromDate = DateTime.ParseExact(from, _settings.Value.ApiRequestDateFormat, CultureInfo.InvariantCulture);
                var toDate = DateTime.ParseExact(to, _settings.Value.ApiRequestDateFormat, CultureInfo.InvariantCulture);
                var logs = await _requestAttemptStorage.GetRecordsWithinIntervalAsync(fromDate, toDate, cancellationToken);
                return new OkObjectResult(logs);
            }
            catch (FormatException e)
            {
                return new BadRequestObjectResult("Invalid query parameters");
            }
        }

        #region Constants
        private const string FromQueryParameterName = "from";
        private const string ToQueryParameterName = "to";
        #endregion
        
        #region Fields
        private readonly IRequestAttemptStorage _requestAttemptStorage;
        private readonly IOptions<ApplicationSettings> _settings;
        #endregion
    }
}