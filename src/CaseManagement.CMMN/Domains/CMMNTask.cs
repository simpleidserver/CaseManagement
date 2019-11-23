using CaseManagement.CMMN.Domains.Events;

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

        public override void Handle(DomainEvent cmmnPlanItemEvent)
        {
            if (cmmnPlanItemEvent is CMMNPlanItemCreated)
            {
                Handle((CMMNPlanItemCreated)cmmnPlanItemEvent);
            }

            if (cmmnPlanItemEvent is CMMNPlanItemEnabled)
            {
                Handle((CMMNPlanItemEnabled)cmmnPlanItemEvent);
            }

            if (cmmnPlanItemEvent is CMMNPlanItemManuallyStarted)
            {
                Handle((CMMNPlanItemManuallyStarted)cmmnPlanItemEvent);
            }
        }

        private void Handle(CMMNPlanItemCreated evt)
        {
            State = CMMNTaskStates.Available;
        }

        private void Handle(CMMNPlanItemEnabled evt)
        {
            State = CMMNTaskStates.Enabled;
        }

        private void Handle(CMMNPlanItemManuallyStarted evt)
        {
            State = CMMNTaskStates.Active;
        }
    }
}
