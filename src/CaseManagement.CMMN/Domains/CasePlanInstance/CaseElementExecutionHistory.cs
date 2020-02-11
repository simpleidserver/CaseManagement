using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseElementExecutionHistory : ICloneable
    {
        public CaseElementExecutionHistory(string workflowElementDefinitionId, DateTime startDateTime)
        {
            CaseElementDefinitionId = workflowElementDefinitionId;
            StartDateTime = startDateTime;
        }

        public string CaseElementDefinitionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public object Clone()
        {
            return new CaseElementExecutionHistory(CaseElementDefinitionId, StartDateTime)
            {
                EndDateTime = EndDateTime
            };
        }
    }
}
