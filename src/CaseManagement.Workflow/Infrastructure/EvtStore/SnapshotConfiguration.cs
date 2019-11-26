namespace CaseManagement.Workflow.Infrastructure.EvtStore
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
