using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConsumeCMMNTransitionEvent
{
    public class CMMNTransitionEventMessage
    {
        public string Id { get; set; }
        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
        public CMMNTransitions Transition { get; set; }
    }
}
