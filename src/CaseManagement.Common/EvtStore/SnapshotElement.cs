using CaseManagement.Common.Domains;
using System;

namespace CaseManagement.Common.EvtStore
{
    public class SnapshotElement<T> where T : BaseAggregate
    {
        public SnapshotElement() { }

        public SnapshotElement(long start, DateTime createDateTime, string id, T content)
        {
            Start = start;
            CreateDateTime = createDateTime;
            Id = id;
            Content = content;
        }

        public long Start { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Id { get; set; }
        public T Content { get; set; }
    }
}
