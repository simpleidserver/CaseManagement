namespace CaseManagement.CMMN
{
    public class CMMNServerOptions
    {
        public CMMNServerOptions()
        {
            PerformanceIntervalInSeconds = 4;
            BlockThreadMS = 20;
            MaxNbPerformanceRecords = 10;
        }

        /// <summary>
        /// Set the interval time in seconds to compute the performance.
        /// </summary>
        public int PerformanceIntervalInSeconds { get; set; }
        /// <summary>
        /// Set the time in milliseconds used to block a thread.
        /// </summary>
        public int BlockThreadMS { get; set; }
        /// <summary>
        /// Set the maximum number of records (performance).
        /// </summary>
        public int MaxNbPerformanceRecords { get; set; }
    }
}
