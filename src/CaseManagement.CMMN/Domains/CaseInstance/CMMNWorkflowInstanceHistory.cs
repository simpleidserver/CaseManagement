using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowInstanceHistory
    {
        public CMMNWorkflowInstanceHistory(string state, DateTime updateDateTime)
        {
            State = state;
            UpdateDateTime = updateDateTime;
        }

        public string State { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
