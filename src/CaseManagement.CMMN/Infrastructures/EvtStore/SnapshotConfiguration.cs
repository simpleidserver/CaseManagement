namespace CaseManagement.CMMN.Infrastructures.EvtStore
{
    public class SnapshotConfiguration
    {
        public SnapshotConfiguration()
        {
            SnapshotFrequency = 100;
        }

        public int SnapshotFrequency { get; set; }
    }
}
