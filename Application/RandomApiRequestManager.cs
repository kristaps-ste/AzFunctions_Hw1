using Application.RandomApiRequest;
using Application.Storage;
using Domain;

namespace Application
{
    public class RandomApiRequestManager : IRandomApiRequestManager
    {
        public RandomApiRequestManager(IContentStorage contentStorage,
            IRequestAttemptStorage requestAttemptStorage,
            IRandomApiRequestService randomApiRequestService)
        {
            _contentStorage = contentStorage;
            _requestAttemptStorage = requestAttemptStorage;
            _randomApiRequestService = randomApiRequestService;
        }

        public async Task ProcessRequestAsync(CancellationToken cancellationToken)
        {
            RequestAttemptResult result;
            try
            {
                var content = await _randomApiRequestService.RequestRandomApiContentAsync();
                result = RequestAttemptResult.CreateSuccessful(content);
            }
            catch (RequestFailedException)
            {
                result = RequestAttemptResult.CreateFailed();
            }
            
            if (result is { Success: true, Content: not null })
            {
                await _contentStorage.UploadRequestContentAsync(result, true);
            }
            
            await _requestAttemptStorage.SaveRequestAttempt(result);
        }

        #region Fields
        private readonly IContentStorage _contentStorage;
        private readonly IRequestAttemptStorage _requestAttemptStorage;
        private readonly IRandomApiRequestService _randomApiRequestService;
        #endregion
    }
}