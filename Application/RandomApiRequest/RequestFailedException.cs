namespace Application.RandomApiRequest
{
    public class RequestFailedException : Exception
    {
        public RequestFailedException(string message) : base(message)
        {
            
        }
    }
}