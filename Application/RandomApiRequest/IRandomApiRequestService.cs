using Domain;

namespace Application.RandomApiRequest
{
    public interface IRandomApiRequestService
    {
        Task<string> RequestRandomApiContentAsync();
    }
}