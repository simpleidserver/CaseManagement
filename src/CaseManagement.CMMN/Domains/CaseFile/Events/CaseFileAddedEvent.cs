using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Case file '{AggregateId}' is added by '{Owner}'")]
    public class CaseFileAddedEvent : DomainEvent
    {
        public CaseFileAddedEvent(string id, string aggregateId, int version, string fileId, string name, string description, DateTime createDateTime, string owner, string payload) : base(id, aggregateId, version)
        {
            FileId = fileId;
            Name = name;
            Description = description;
            CreateDateTime = createDateTime;
            Owner = owner;
            Payload = payload;
        }

        public string FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Owner { get; set; }
        public string Payload { get; set; }
    }
}