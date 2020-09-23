using System;

namespace CaseManagement.BPMN.Domains
{
    public class ActivityStateHistory : ICloneable
    {
        public ActivityStateHistory() { }

        public ActivityStateHistory(ActivityStates state, DateTime executionDateTime)
        {
            State = state;
            ExecutionDateTime = executionDateTime;
        }

        public ActivityStates State { get; set; }
        public DateTime ExecutionDateTime { get; set; }

        public object Clone()
        {
            return new ActivityStateHistory(State, ExecutionDateTime);
        }
    }
}
