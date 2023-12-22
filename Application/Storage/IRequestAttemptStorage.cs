using Domain;

namespace Application.Storage
{
    public interface IRequestAttemptStorage
    {
        Task SaveRequestAttempt(RequestAttemptResult attemptResult);
        Task<List<RequestAttemptLog>> GetRecordsWithinIntervalAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}