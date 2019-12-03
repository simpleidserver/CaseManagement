namespace CaseManagement.CMMN.Domains
{
    public class CMMNTask : CMMNPlanItemDefinition
    {
        public CMMNTask(string name) : base(name)
        {
            State = CMMNTaskStates.Available;
        }

        public CMMNTaskStates State { get; set; }
        /// <summary>
        /// The Task is waiting until the work associated with the Task is completed.
        /// </summary>
        public bool IsBlocking { get; set; }

        public override void Handle(CMMNPlanItemTransitions transition)
        {
            switch(transition)
            {
                case CMMNPlanItemTransitions.Create:
                    State = CMMNTaskStates.Available;
                    break;
                case CMMNPlanItemTransitions.Enable:
                    State = CMMNTaskStates.Enabled;
                    break;
                case CMMNPlanItemTransitions.ManualStart:
                    State = CMMNTaskStates.Active;
                    break;
                case CMMNPlanItemTransitions.Start:
                    State = CMMNTaskStates.Active;
                    break;
                case CMMNPlanItemTransitions.Terminate:
                    State = CMMNTaskStates.Terminated;
                    break;
                case CMMNPlanItemTransitions.Complete:
                    State = CMMNTaskStates.Completed;
                    break;
            }
        }

        public override object Clone()
        {
            return CloneTask();
        }

        public virtual object CloneTask()
        {
            return new CMMNTask(Name)
            {
                State = State,
                IsBlocking = IsBlocking
            };
        }
    }
}
