using System.Net.Sockets;
using Application.RandomApiRequest;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace RandomApiRequest.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRandomApiRequests(this IServiceCollection services)
        {
            services.AddTransient<IRandomApiRequestService, RandomApiRequestService>();
        
            var httpGetRetryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .Or<TaskCanceledException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount) / 2));
            
            services.AddHttpClient(nameof(IRandomApiRequestService)).AddPolicyHandler(httpGetRetryPolicy);
            return services;
            
        }
    }
}