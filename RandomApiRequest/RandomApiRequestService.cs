using Application.Configuration;
using Application.RandomApiRequest;
using Microsoft.Extensions.Options;
using RandomApiRequest.Configuration;

namespace RandomApiRequest
{
    public class RandomApiRequestService : IRandomApiRequestService
    {

        public RandomApiRequestService(IHttpClientFactory clientFactory, IOptions<RandomApiSettings> settings)
        {
            _clientFactory = clientFactory;
            _settings = settings;
        }

        #region Fields
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<RandomApiSettings> _settings;
        #endregion

        public async Task<string> RequestRandomApiContentAsync()
        {
            _settings.Value.RequestApiUrl = "https://api.publicapis.org/random?auth=null";
            var url =  new Uri(_settings.Value.RequestApiUrl);
            var client = _clientFactory.CreateClient(nameof(IRandomApiRequestService));
            var request = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new RequestFailedException($"Request failed with status code: {response.StatusCode}");
            }
            
            return  await response.Content.ReadAsStringAsync();
        }
    }
}