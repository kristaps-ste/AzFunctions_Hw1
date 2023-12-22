using Application.Storage;
using Azure.Data.Tables;
using AzureStorage.TableStorageService.LogTableEntity;
using Domain;

namespace AzureStorage.TableStorageService
{
    public class TableStorageService : IRequestAttemptStorage
    {
        public TableStorageService(TableClient tableClient)
        {
            _tableClient = tableClient;
        }
        
        public async Task SaveRequestAttempt(RequestAttemptResult requestAttemptResultResult)
        {
            var logEntity = requestAttemptResultResult.ToLogEntity();
            await _tableClient.AddEntityAsync(logEntity);
        }

        public async Task<List<RequestAttemptLog>> GetRecordsWithinIntervalAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var items = new List<LogEntity>();
            var query = $"PartitionKey eq '{nameof(LogEntity)}' and RowKey gt '{from.Ticks}' and RowKey lt '{to.Ticks}'";
            var queryResult = _tableClient.QueryAsync<LogEntity>(query, cancellationToken: cancellationToken);
            await foreach (var page in queryResult.AsPages().WithCancellation(cancellationToken))
            {
                items.AddRange(page.Values);
            }

            return items.ToRequestAttemptLog();
        }

        #region Fields
        private readonly TableClient _tableClient;
        #endregion
    }
}