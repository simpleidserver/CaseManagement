using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [DebuggerDisplay("Case file {AggregateId} is published")]
    [Serializable]
    public class CaseFilePublishedEvent : DomainEvent
    {
        public CaseFilePublishedEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
