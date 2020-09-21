using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("remove worker task")]
    public class CaseInstanceWorkerTaskRemovedEvent : DomainEvent
    {
        public CaseInstanceWorkerTaskRemovedEvent(string id, string aggregateId, int version, string casePlanInstanceElementId, DateTime executionDateTime) : base(id, aggregateId, version)
        {
            CasePlanInstanceElementId = casePlanInstanceElementId;
            ExecutionDateTime = executionDateTime;
        }

        public string CasePlanInstanceElementId { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }
}
