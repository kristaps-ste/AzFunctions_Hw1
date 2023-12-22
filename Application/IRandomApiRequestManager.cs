namespace Application
{
    public interface IRandomApiRequestManager
    {
        /// <summary>
        /// Process the request for random api data collection;
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task ProcessRequestAsync(CancellationToken cancellationToken);
    }
}