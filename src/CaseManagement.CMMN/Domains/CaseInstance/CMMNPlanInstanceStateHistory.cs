using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanInstanceStateHistory
    {
        public CMMNPlanInstanceStateHistory(string state, DateTime updateDateTime)
        {
            State = state;
            UpdateDateTime = updateDateTime;
        }

        public string State { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
