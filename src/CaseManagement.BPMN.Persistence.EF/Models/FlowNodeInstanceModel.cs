using CaseManagement.BPMN.Domains;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class FlowNodeInstanceModel
    {
        public string Id { get; set; }
        public string FlowNodeId { get; set; }
        public FlowNodeStates State { get; set; }
        public ActivityStates? ActivityState { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public virtual ICollection<ActivityStateHistoryModel> ActivityStates { get; set; }
    }
}
