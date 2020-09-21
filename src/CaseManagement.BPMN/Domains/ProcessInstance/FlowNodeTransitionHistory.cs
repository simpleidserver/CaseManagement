using System;

namespace CaseManagement.BPMN.Domains
{
    public class FlowNodeTransitionHistory
    {
        public BPMNTransitions Transition { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }
}
