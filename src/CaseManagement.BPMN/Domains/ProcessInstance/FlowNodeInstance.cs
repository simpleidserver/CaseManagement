using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class FlowNodeInstance : ICloneable
    {
        public FlowNodeInstance()
        {
            Metadata = new Dictionary<string, string>();
            ActivityStates = new List<ActivityStateHistory>();
        }

        public string EltId { get; set; }
        public string FlowNodeId { get; set; }
        public FlowNodeStates State { get; set; }
        public ActivityStates? ActivityState { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public ICollection<ActivityStateHistory> ActivityStates { get; set; }

        public object Clone()
        {
            return new FlowNodeInstance
            {
                EltId = EltId,
                FlowNodeId = FlowNodeId,
                State = State,
                ActivityState = ActivityState,
                Metadata = Metadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                ActivityStates = ActivityStates.Select(_ => (ActivityStateHistory)_.Clone()).ToList()
            };
        }

        public void UpdateState(ActivityStates state, DateTime updateDateTime, string message = null)
        {
            ActivityStates.Add(new ActivityStateHistory(state, updateDateTime, message));
            ActivityState = state;
        }
    }
}
