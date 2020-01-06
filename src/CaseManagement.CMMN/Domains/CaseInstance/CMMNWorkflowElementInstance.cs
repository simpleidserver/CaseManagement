using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowElementInstance : ICloneable
    {
        public CMMNWorkflowElementInstance(string id, DateTime createDateTime, string workflowElementDefinitionId, CMMNWorkflowElementTypes workflowElementDefinitionType, int version, string parentId)
        {
            Id = id;
            CreateDateTime = createDateTime;
            WorkflowElementDefinitionId = workflowElementDefinitionId;
            WorkflowElementDefinitionType = workflowElementDefinitionType;
            Version = version;
            StateHistories = new List<CMMNWorkflowElementInstanceHistory>();
            TransitionHistories = new List<CMMNWorkflowElementInstanceTransitionHistory>();
            ParentId = parentId;
        }

        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string WorkflowElementDefinitionId { get; set; }
        public CMMNWorkflowElementTypes WorkflowElementDefinitionType { get; set; }
        public string FormInstanceId { get; set; }
        public string State { get; set; }
        public string ParentId { get; set; }
        public ICollection<CMMNWorkflowElementInstanceHistory> StateHistories { get; set; }
        public ICollection<CMMNWorkflowElementInstanceTransitionHistory> TransitionHistories { get; set; }
        public event EventHandler<string> TransitionApplied;

        public void UpdateState(CMMNTransitions transition, DateTime updateDateTime)
        {
            lock(TransitionHistories)
            {
                lock(StateHistories)
                {
                    var state = GetState(transition);
                    StateHistories.Add(new CMMNWorkflowElementInstanceHistory(state, updateDateTime));
                    TransitionHistories.Add(new CMMNWorkflowElementInstanceTransitionHistory(transition, updateDateTime));
                    State = state;
                    if (TransitionApplied != null)
                    {
                        TransitionApplied(this, state);
                    }
                }
            }
        }

        public static CMMNWorkflowElementInstance New(CMMNWorkflowElementDefinition workflowElementDefinition)
        {
            return new CMMNWorkflowElementInstance(Guid.NewGuid().ToString(), DateTime.UtcNow, workflowElementDefinition.Id, workflowElementDefinition.Type, 0, null);
        }

        public bool IsFail()
        {
            switch (WorkflowElementDefinitionType)
            {
                case CMMNWorkflowElementTypes.HumanTask:
                case CMMNWorkflowElementTypes.ProcessTask:
                case CMMNWorkflowElementTypes.Stage:
                case CMMNWorkflowElementTypes.Task:
                    if (State == Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed))
                    {
                        return true;
                    }

                    return false;
            }

            return false;
        }

        public bool IsSuspend()
        {
            switch (WorkflowElementDefinitionType)
            {
                case CMMNWorkflowElementTypes.HumanTask:
                case CMMNWorkflowElementTypes.ProcessTask:
                case CMMNWorkflowElementTypes.Stage:
                case CMMNWorkflowElementTypes.Task:
                    if (State == Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Suspended))
                    {
                        return true;
                    }

                    return false;
                case CMMNWorkflowElementTypes.Milestone:
                case CMMNWorkflowElementTypes.TimerEventListener:
                    if (State == Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Suspended))
                    {
                        return true;
                    }

                    return false;
            }

            return false;
        }

        public bool IsActive()
        {
            switch (WorkflowElementDefinitionType)
            {
                case CMMNWorkflowElementTypes.CaseFileItem:
                    if (State == Enum.GetName(typeof(CMMNCaseFileItemStates), CMMNCaseFileItemStates.Available))
                    {
                        return true;
                    }

                    return false;
                case CMMNWorkflowElementTypes.HumanTask:
                case CMMNWorkflowElementTypes.ProcessTask:
                case CMMNWorkflowElementTypes.Stage:
                case CMMNWorkflowElementTypes.Task:
                    if (State == Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active))
                    {
                        return true;
                    }

                    return false;
                case CMMNWorkflowElementTypes.Milestone:
                case CMMNWorkflowElementTypes.TimerEventListener:
                    if (State == Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available))
                    {
                        return true;
                    }

                    return false;
            }

            return false;
        }

        public bool IsComplete()
        {
            switch (WorkflowElementDefinitionType)
            {
                case CMMNWorkflowElementTypes.CaseFileItem:
                    if (State == Enum.GetName(typeof(CMMNCaseFileItemStates), CMMNCaseFileItemStates.Discarded))
                    {
                        return true;
                    }

                    return false;
                case CMMNWorkflowElementTypes.HumanTask:
                case CMMNWorkflowElementTypes.ProcessTask:
                case CMMNWorkflowElementTypes.Stage:
                case CMMNWorkflowElementTypes.Task:
                    if (State == Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed) || State == Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated))
                    {
                        return true;
                    }

                    return false;
                case CMMNWorkflowElementTypes.Milestone:
                case CMMNWorkflowElementTypes.TimerEventListener:
                    if (State == Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed) || State == Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Terminated))
                    {
                        return true;
                    }

                    return false;
            }

            return false;
        }

        private string GetState(CMMNTransitions planItemTransition)
        {
            switch(WorkflowElementDefinitionType)
            {
                case CMMNWorkflowElementTypes.HumanTask:
                case CMMNWorkflowElementTypes.Task:
                case CMMNWorkflowElementTypes.ProcessTask:
                case CMMNWorkflowElementTypes.Stage:
                    CMMNTaskStates taskState = CMMNTaskStates.Available;
                    switch (planItemTransition)
                    {
                        case CMMNTransitions.Create:
                            if (!string.IsNullOrWhiteSpace(State))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is already initialized" }
                                });
                            }

                            taskState = CMMNTaskStates.Available;
                            break;
                        case CMMNTransitions.Enable:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not available" }
                                });
                            }

                            taskState = CMMNTaskStates.Enabled;
                            break;
                        case CMMNTransitions.Disable:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Enabled))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not enabled" }
                                });
                            }

                            taskState = CMMNTaskStates.Disabled;
                            break;
                        case CMMNTransitions.Reenable:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Disabled))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not disabled" }
                                });
                            }

                            taskState = CMMNTaskStates.Enabled;
                            break;
                        case CMMNTransitions.Fault:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not active" }
                                });
                            }

                            taskState = CMMNTaskStates.Failed;
                            break;
                        case CMMNTransitions.Reactivate:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not failed" }
                                });
                            }

                            taskState = CMMNTaskStates.Active;
                            break;
                        case CMMNTransitions.ManualStart:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Enabled))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not enabled" }
                                });
                            }

                            taskState = CMMNTaskStates.Active;
                            break;
                        case CMMNTransitions.Start:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not available" }
                                });
                            }

                            taskState = CMMNTaskStates.Active;
                            break;
                        case CMMNTransitions.Terminate:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not active" }
                                });
                            }

                            taskState = CMMNTaskStates.Terminated;
                            break;
                        case CMMNTransitions.Suspend:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not active" }
                                });
                            }

                            taskState = CMMNTaskStates.Suspended;
                            break;
                        case CMMNTransitions.Resume:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Suspended))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not suspended" }
                                });
                            }

                            taskState = CMMNTaskStates.Active;
                            break;
                        case CMMNTransitions.Complete:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not active" }
                                });
                            }

                            taskState = CMMNTaskStates.Completed;
                            break;
                        case CMMNTransitions.Exit:
                        case CMMNTransitions.ParentExit:
                            taskState = CMMNTaskStates.Terminated;
                            break;
                        case CMMNTransitions.ParentTerminate:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not active" }
                                });
                            }

                            taskState = CMMNTaskStates.Terminated;
                            break;
                        case CMMNTransitions.ParentSuspend:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not active" }
                                });
                            }

                            taskState = CMMNTaskStates.Suspended;
                            break;
                        case CMMNTransitions.ParentResume:
                            if (State != Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Suspended))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "planitem instance is not suspended" }
                                });
                            }

                            taskState = CMMNTaskStates.Active;
                            break;
                    }

                    return Enum.GetName(typeof(CMMNTaskStates), taskState);
                case CMMNWorkflowElementTypes.Milestone:
                case CMMNWorkflowElementTypes.TimerEventListener:
                    CMMNMilestoneStates milestoneState = CMMNMilestoneStates.Available;
                    switch (planItemTransition)
                    {
                        case CMMNTransitions.Create:
                            if (!string.IsNullOrWhiteSpace(State))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "milestone instance is initialized" }
                                });
                            }

                            milestoneState = CMMNMilestoneStates.Available;
                            break;
                        case CMMNTransitions.Occur:
                            if (State != Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "milestone instance is not available" }
                                });
                            }

                            milestoneState = CMMNMilestoneStates.Completed;
                            break;
                        case CMMNTransitions.Suspend:
                            if (State != Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "milestone instance is not available" }
                                });
                            }

                            milestoneState = CMMNMilestoneStates.Suspended;
                            break;
                        case CMMNTransitions.Terminate:
                            if (State != Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "milestone instance is not available" }
                                });
                            }

                            milestoneState = CMMNMilestoneStates.Terminated;
                            break;
                        case CMMNTransitions.Resume:
                            if (State != Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Suspended))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "milestone instance is not suspended" }
                                });
                            }

                            milestoneState = CMMNMilestoneStates.Available;
                            break;
                    }

                    return Enum.GetName(typeof(CMMNMilestoneStates), milestoneState);
                case CMMNWorkflowElementTypes.CaseFileItem:
                    CMMNCaseFileItemStates caseFileItemState = CMMNCaseFileItemStates.Available;
                    switch(planItemTransition)
                    {
                        case CMMNTransitions.Create:
                            if (!string.IsNullOrWhiteSpace(State))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "casefileitem is initialized" }
                                });
                            }

                            caseFileItemState = CMMNCaseFileItemStates.Available;
                            break;
                        case CMMNTransitions.Update:
                        case CMMNTransitions.Replace:
                        case CMMNTransitions.AddChild:
                        case CMMNTransitions.RemoveChild:
                        case CMMNTransitions.AddReference:
                        case CMMNTransitions.RemoveReference:
                            if (State != Enum.GetName(typeof(CMMNCaseFileItemStates), CMMNCaseFileItemStates.Available))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "casefileitem is not available" }
                                });
                            }

                            caseFileItemState = CMMNCaseFileItemStates.Available;
                            break;
                        case CMMNTransitions.Delete:
                            if (State != Enum.GetName(typeof(CMMNCaseFileItemStates), CMMNCaseFileItemStates.Available))
                            {
                                throw new AggregateValidationException(new Dictionary<string, string>
                                {
                                    { "transition", "casefileitem is not available" }
                                });
                            }

                            caseFileItemState = CMMNCaseFileItemStates.Discarded;
                            break;
                    }
                    return Enum.GetName(typeof(CMMNCaseFileItemStates), caseFileItemState);
            }

            return null;
        }

        public object Clone()
        {
            return new CMMNWorkflowElementInstance(Id, CreateDateTime, WorkflowElementDefinitionId, WorkflowElementDefinitionType, Version, ParentId)
            {
                FormInstanceId = FormInstanceId,
                State = State,
                StateHistories = StateHistories == null ? null : StateHistories.Select(s => (CMMNWorkflowElementInstanceHistory)s.Clone()).ToList(),
                TransitionHistories = TransitionHistories == null ? null : TransitionHistories.Select(t => (CMMNWorkflowElementInstanceTransitionHistory)t.Clone()).ToList()
            };
        }
    }
}
