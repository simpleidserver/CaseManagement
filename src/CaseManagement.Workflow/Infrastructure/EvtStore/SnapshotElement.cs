using System;

namespace CaseManagement.Workflow.Infrastructure.EvtStore
{
    public class SnapshotElement<T> where T : BaseAggregate
    {
        public SnapshotElement() { }

        public SnapshotElement(long start, DateTime createDateTime, T content)
        {
            Start = start;
            CreateDateTime = createDateTime;
            Content = content;
        }

        public long Start { get; set; }
        public DateTime CreateDateTime { get; set; }
        public T Content { get; set; }
    }
}
