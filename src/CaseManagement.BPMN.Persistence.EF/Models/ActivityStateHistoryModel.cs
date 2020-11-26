using CaseManagement.BPMN.Domains;
using System;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class ActivityStateHistoryModel
    {
        public long Id { get; set; }
        public ActivityStates State { get; set; }
        public string Message { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }
}
