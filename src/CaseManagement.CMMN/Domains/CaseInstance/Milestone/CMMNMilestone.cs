namespace CaseManagement.CMMN.Domains
{
    public class CMMNMilestone : CMMNPlanItemDefinition
    {
        public CMMNMilestone() { }

        public CMMNMilestone(string name) : base(name) { }

        public CMMNMilestoneStates State { get; set; }

        public override object Clone()
        {
            return new CMMNMilestone
            {
                Name = Name,
                State = State
            };
        }

        public override void Handle(CMMNPlanItemTransitions transition)
        {
            switch(transition)
            {
                case CMMNPlanItemTransitions.Create:
                    State = CMMNMilestoneStates.Available;
                    break;
                case CMMNPlanItemTransitions.Occur:
                    State = CMMNMilestoneStates.Completed;
                    break;
                case CMMNPlanItemTransitions.Suspend:
                    State = CMMNMilestoneStates.Suspended;
                    break;
                case CMMNPlanItemTransitions.Terminate:
                    State = CMMNMilestoneStates.Terminated;
                    break;
            }
        }
    }
}
