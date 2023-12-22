namespace Domain
{
    public class RequestAttemptLog
    {
        
        public static RequestAttemptLog CreateFromId(long id, bool success)
        {
            var requestTime = new DateTime(id);
            return new RequestAttemptLog(requestTime, success, id);
        }
        
        public DateTime RequestTime { get; private set; }
        public bool Success { get; private set; }
        public long Id { get; set;}
        
        private RequestAttemptLog(DateTime requestTime, bool success, long id)
        {
            RequestTime = requestTime;
            Success = success;
            Id = id;
        }
    }
}