namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public class BusOptions
    {
        public BusOptions()
        {
            MaxConcurrentTask = 20;
            IdleTimeInMs = 10;
            ConcurrencyExceptionIdleTimeInMs = 30;
            ConcurrencyExceptionMaxRetry = 20;
        }

        public int MaxConcurrentTask { get; set; }
        public int IdleTimeInMs { get; set; }
        public int ConcurrencyExceptionIdleTimeInMs { get; set; }
        public int ConcurrencyExceptionMaxRetry { get; set; }
    }
}
