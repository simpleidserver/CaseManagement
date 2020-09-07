using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public abstract class CasePlanElementInstance
    {
        public CasePlanElementInstance()
        {
            EntryCriterions = new List<Criteria>();
            ExitCriterions = new List<Criteria>();
            TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public RepetitionRule RepetitionRule { get; set; }

        public ConcurrentBag<CasePlanElementInstanceTransitionHistory> TransitionHistories { get; set; }
        /// <summary>
        /// Zero or more EntryCriterion for that PlanItem. [0...*].
        /// An EntryCriterion represents the condition for a PlanItem to become available.
        /// </summary>
        public ICollection<Criteria> EntryCriterions { get; set; }
        public ICollection<Criteria> ExitCriterions { get; set; }

        public CMMNTransitions? LatestTransition => TransitionHistories.OrderByDescending(_ => _.ExecutionDateTime).FirstOrDefault()?.Transition;

        public void MakeTransition(CMMNTransitions transition, DateTime executionDateTime)
        {
            UpdateTransition(transition, executionDateTime);
            TransitionHistories.Add(new CasePlanElementInstanceTransitionHistory { ExecutionDateTime = executionDateTime, Transition = transition });
        }

        protected abstract void UpdateTransition(CMMNTransitions transition, DateTime executionDateTime);

        protected TaskStageStates? GetTaskStageState(TaskStageStates? state, CMMNTransitions transition)
        {
            TaskStageStates? result = null;
            switch (transition)
            {
                case CMMNTransitions.Create:
                    if (state != null)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "already initialized" }
                        });
                    }

                    result = TaskStageStates.Available;
                    break;
                case CMMNTransitions.Enable:
                    if (state != TaskStageStates.Available)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not available" }
                        });
                    }

                    result = TaskStageStates.Enabled;
                    break;
                case CMMNTransitions.Disable:
                    if (state != TaskStageStates.Enabled)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not enabled" }
                        });
                    }

                    result = TaskStageStates.Disabled;
                    break;
                case CMMNTransitions.Reenable:
                    if (state != TaskStageStates.Enabled)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not disabled" }
                        });
                    }

                    result = TaskStageStates.Disabled;
                    break;
                case CMMNTransitions.Fault:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not active" }
                        });
                    }

                    result = TaskStageStates.Failed;
                    break;
                case CMMNTransitions.Reactivate:
                    if (state != TaskStageStates.Failed)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not failed" }
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.ManualStart:
                    if (state != TaskStageStates.Enabled)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not enabled" }
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.Start:
                    if (state != TaskStageStates.Available)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not available" }
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.Terminate:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not active" }
                        });
                    }

                    result = TaskStageStates.Terminated;
                    break;
                case CMMNTransitions.Suspend:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not active" }
                        });
                    }

                    result = TaskStageStates.Suspended;
                    break;
                case CMMNTransitions.Resume:
                    if (state != TaskStageStates.Suspended)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not suspended" }
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.Complete:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "planitem instance is not active" }
                        });
                    }

                    result = TaskStageStates.Completed;
                    break;
            }

            return result;
        }

        protected MilestoneEventStates? GetMilestoneOrEventListenerState(MilestoneEventStates? state, CMMNTransitions transition)
        {
            MilestoneEventStates? result = null;
            switch (transition)
            {
                case CMMNTransitions.Create:
                    if (state != null)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "already initialized" }
                        });
                    }

                    result = MilestoneEventStates.Available;
                    break;
                case CMMNTransitions.Occur:
                    if (state != MilestoneEventStates.Available)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "milestone instance is not available" }
                        });
                    }

                    result = MilestoneEventStates.Completed;
                    break;
                case CMMNTransitions.Suspend:
                    if (state != MilestoneEventStates.Available)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "milestone instance is not available" }
                        });
                    }

                    result = MilestoneEventStates.Suspended;
                    break;
                case CMMNTransitions.Terminate:
                    if (state != MilestoneEventStates.Available)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "milestone instance is not available" }
                        });
                    }

                    result = MilestoneEventStates.Terminated;
                    break;
                case CMMNTransitions.Resume:
                    if (state != MilestoneEventStates.Suspended)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "milestone instance is not suspended" }
                        });
                    }

                    result = MilestoneEventStates.Available;
                    break;
            }

            return result;
        }
    }
}
