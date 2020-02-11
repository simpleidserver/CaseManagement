using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Infrastructures.Bus.Transition
{
    public class TransitionCommand
    {
        public string Id { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public CMMNTransitions Transition { get; set; }
    }
}
