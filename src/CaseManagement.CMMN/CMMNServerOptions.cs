namespace CaseManagement.CMMN
{
    public class CMMNServerOptions
    {
        public CMMNServerOptions()
        {
            MaxConcurrentTask = 20;
            PerformanceIntervalInSeconds = 4;
            BlockThreadMS = 20;
            MaxNbPerformanceRecords = 10;
            SnapshotFrequency = 100;
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
        /// <summary>
        /// Set the maximum of concurrent tasks.
        /// </summary>
        public int MaxConcurrentTask { get; set; }
        /// <summary>
        /// Set the snapshot frequency.
        /// </summary>
        public int SnapshotFrequency { get; set; }
    }
}
