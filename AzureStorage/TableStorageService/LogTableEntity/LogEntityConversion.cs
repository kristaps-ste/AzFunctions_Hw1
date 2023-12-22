using Domain;

namespace AzureStorage.TableStorageService.LogTableEntity
{
    public static class LogEntityConversion
    {
        public static LogEntity ToLogEntity(this RequestAttemptResult requestAttemptResult)
        {
            return new LogEntity()
            {
                PartitionKey = nameof(LogEntity),
                RowKey = requestAttemptResult.Id.ToString(),
                Success = requestAttemptResult.Success
            };
        }
        
        public static RequestAttemptLog ToRequestAttemptLog(this LogEntity logEntity)
        {
            var isSuccess = long.TryParse(logEntity.RowKey, out long id);
            if (!isSuccess)
            {
                throw new ArgumentException($"Failed to parse {logEntity.RowKey} to long");
            }
            
            return RequestAttemptLog.CreateFromId(id, logEntity.Success);
        }

        public static List<RequestAttemptLog> ToRequestAttemptLog(this List<LogEntity> logEntities) =>
            logEntities.Select(e => e.ToRequestAttemptLog()).ToList();
        
    }
}