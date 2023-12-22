namespace Domain
{
    public class RequestAttemptResult
    {
        public static RequestAttemptResult CreateSuccessful(string content)
        {
            return new RequestAttemptResult
            {
                RequestTime = DateTime.UtcNow,
                Success = true,
                Content = content
            };
        }
        
        public static RequestAttemptResult CreateFailed()
        {
            return new RequestAttemptResult
            {
                RequestTime = DateTime.UtcNow,
                Success = false
            };
        }
        
        public DateTime RequestTime { get; private set; }
        public bool Success  { get; private set; }
        public string? Content { get; private set; }
        public long Id => RequestTime.Ticks;
    }
}