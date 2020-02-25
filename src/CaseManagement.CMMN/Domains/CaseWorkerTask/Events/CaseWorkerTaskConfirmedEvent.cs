using CaseManagement.CMMN.Infrastructures;
using System;

namespace CaseManagement.CMMN.Domains.Events
{
    public class CaseWorkerTaskConfirmedEvent : DomainEvent
    {
        public CaseWorkerTaskConfirmedEvent(string id, string aggregateId, int version, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UpdateDateTime = updateDateTime;
        }

        
        public DateTime UpdateDateTime { get; set; }
    }
}
