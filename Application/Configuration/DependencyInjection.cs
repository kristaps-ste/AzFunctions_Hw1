using Application.RandomApiRequest;
using Application.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IRandomApiRequestManager, RandomApiRequestManager>();
            return services;
        }
    }
}