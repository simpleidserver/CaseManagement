using CaseManagement.CMMN.Infrastructures;
using System;

namespace CaseManagement.CMMN.Domains.Events
{
    public class CaseWorkerTaskAddedEvent : DomainEvent
    {
        public CaseWorkerTaskAddedEvent(string id, string aggregateId, int version, string performerRole, string casePlanId, string casePlanInstanceId, string casePlanElementInstanceId, CaseWorkerTaskTypes taskType, DateTime createDateTime) : base(id, aggregateId, version)
        {
            PerformerRole = performerRole;
            CasePlanId = casePlanId;
            CasePlanInstanceId = casePlanInstanceId;
            CasePlanElementInstanceId = casePlanElementInstanceId;
            TaskType = taskType;
            CreateDateTime = createDateTime;
        }

        public string PerformerRole { get; set; }
        public string CasePlanId { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public CaseWorkerTaskTypes TaskType { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
