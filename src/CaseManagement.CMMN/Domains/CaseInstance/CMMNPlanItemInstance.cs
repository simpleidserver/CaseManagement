using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItemInstance
    {
        public CMMNPlanItemInstance(string id, DateTime createDateTime, string planItemDefinitionId, CMMNPlanItemDefinitionTypes planItemDefinitionType)
        {
            Id = id;
            CreateDateTime = createDateTime;
            PlanItemDefinitionId = planItemDefinitionId;
            PlanItemDefinitionType = planItemDefinitionType;
            Version = 0;
            StateHistories = new List<CMMNPlanInstanceStateHistory>();
        }

        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string PlanItemDefinitionId { get; set; }
        public CMMNPlanItemDefinitionTypes PlanItemDefinitionType { get; set; }
        public string State { get; set; }
        public ICollection<CMMNPlanInstanceStateHistory> StateHistories { get; set; }
        public event EventHandler<string> TransitionApplied;

        public void UpdateState(CMMNPlanItemTransitions transition, DateTime updateDateTime)
        {
            var state = GetState(transition);
            StateHistories.Add(new CMMNPlanInstanceStateHistory(state, updateDateTime));
            State = state;
            if (TransitionApplied != null)
            {
                TransitionApplied(this, state);
            }
        }

        public static CMMNPlanItemInstance New(CMMNPlanItemDefinition planItemDefinition)
        {
            return new CMMNPlanItemInstance(Guid.NewGuid().ToString(), DateTime.UtcNow, planItemDefinition.Id, planItemDefinition.PlanItemDefinitionType);
        }

        private string GetState(CMMNPlanItemTransitions planItemTransition)
        {
            switch(PlanItemDefinitionType)
            {
                case CMMNPlanItemDefinitionTypes.HumanTask:
                case CMMNPlanItemDefinitionTypes.Task:
                case CMMNPlanItemDefinitionTypes.ProcessTask:
                    CMMNTaskStates taskState = CMMNTaskStates.Available;
                    switch (planItemTransition)
                    {
                        case CMMNPlanItemTransitions.Create:
                            taskState = CMMNTaskStates.Available;
                            break;
                        case CMMNPlanItemTransitions.Enable:
                            taskState = CMMNTaskStates.Enabled;
                            break;
                        case CMMNPlanItemTransitions.ManualStart:
                            taskState = CMMNTaskStates.Active;
                            break;
                        case CMMNPlanItemTransitions.Start:
                            taskState = CMMNTaskStates.Active;
                            break;
                        case CMMNPlanItemTransitions.Terminate:
                            taskState = CMMNTaskStates.Terminated;
                            break;
                        case CMMNPlanItemTransitions.Complete:
                            taskState = CMMNTaskStates.Completed;
                            break;
                    }

                    return Enum.GetName(typeof(CMMNTaskStates), taskState);
                case CMMNPlanItemDefinitionTypes.Milestone:
                    CMMNMilestoneStates milestoneState = CMMNMilestoneStates.Available;
                    switch (planItemTransition)
                    {
                        case CMMNPlanItemTransitions.Create:
                            milestoneState = CMMNMilestoneStates.Available;
                            break;
                        case CMMNPlanItemTransitions.Occur:
                            milestoneState = CMMNMilestoneStates.Completed;
                            break;
                        case CMMNPlanItemTransitions.Suspend:
                            milestoneState = CMMNMilestoneStates.Suspended;
                            break;
                        case CMMNPlanItemTransitions.Terminate:
                            milestoneState = CMMNMilestoneStates.Terminated;
                            break;
                    }

                    return Enum.GetName(typeof(CMMNMilestoneStates), milestoneState);
                case CMMNPlanItemDefinitionTypes.TimerEventListener:
                    var listenerState = CMMNEventListenerStates.Available;
                    switch (planItemTransition)
                    {
                        case CMMNPlanItemTransitions.Create:
                            listenerState = CMMNEventListenerStates.Available;
                            break;
                        case CMMNPlanItemTransitions.Occur:
                            listenerState = CMMNEventListenerStates.Completed;
                            break;
                    }

                    return Enum.GetName(typeof(CMMNPlanItemTransitions), listenerState);
            }

            return null;
        }
    }
}
