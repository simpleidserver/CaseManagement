using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [Serializable]
    [DebuggerDisplay("Case file '{AggregateId}' is published")]
    public class CaseFilePublishedEvent : DomainEvent
    {
        public CaseFilePublishedEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
