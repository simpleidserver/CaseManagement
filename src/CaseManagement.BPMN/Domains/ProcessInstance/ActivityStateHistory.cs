using System;

namespace CaseManagement.BPMN.Domains
{
    public class ActivityStateHistory : ICloneable
    {
        public ActivityStateHistory() { }

        public ActivityStateHistory(ActivityStates state, DateTime executionDateTime, string message = null)
        {
            State = state;
            ExecutionDateTime = executionDateTime;
            Message = message;
        }

        public ActivityStates State { get; set; }
        public string Message { get; set; }
        public DateTime ExecutionDateTime { get; set; }

        public object Clone()
        {
            return new ActivityStateHistory(State, ExecutionDateTime, Message);
        }
    }
}
