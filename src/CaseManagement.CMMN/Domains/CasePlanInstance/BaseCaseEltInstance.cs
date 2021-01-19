using CaseManagement.Common.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public abstract class BaseCaseEltInstance : ICloneable
    {
        public BaseCaseEltInstance()
        {
            TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>();
        }

        #region Properties

        public string Id { get; set; }
        public string EltId { get; set; }
        public string Name { get; set; }
        public ConcurrentBag<CasePlanElementInstanceTransitionHistory> TransitionHistories { get; set; }
        public abstract CasePlanElementInstanceTypes Type { get; }
        public CMMNTransitions? LatestTransition => TransitionHistories.OrderByDescending(_ => _.ExecutionDateTime).FirstOrDefault()?.Transition;

        #endregion

        #region Update state

        public void MakeTransition(CMMNTransitions transition, string message, DateTime executionDateTime)
        {
            UpdateTransition(transition, executionDateTime);
            TransitionHistories.Add(new CasePlanElementInstanceTransitionHistory { Message = message, ExecutionDateTime = executionDateTime, Transition = transition });
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
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "already initialized")
                        });
                    }

                    result = TaskStageStates.Available;
                    break;
                case CMMNTransitions.Enable:
                    if (state != TaskStageStates.Available)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not available")
                        });
                    }

                    result = TaskStageStates.Enabled;
                    break;
                case CMMNTransitions.Disable:
                    if (state != TaskStageStates.Enabled)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not enabled")
                        });
                    }

                    result = TaskStageStates.Disabled;
                    break;
                case CMMNTransitions.Reenable:
                    if (state != TaskStageStates.Disabled)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not disabled")
                        });
                    }

                    result = TaskStageStates.Enabled;
                    break;
                case CMMNTransitions.Fault:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not active")
                        });
                    }

                    result = TaskStageStates.Failed;
                    break;
                case CMMNTransitions.Reactivate:
                    if (state != TaskStageStates.Failed)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not failed")
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.ManualStart:
                    if (state != TaskStageStates.Enabled)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not enabled")
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.Start:
                    if (state != TaskStageStates.Available)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not available")
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.Terminate:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not active")
                        });
                    }

                    result = TaskStageStates.Terminated;
                    break;
                case CMMNTransitions.Suspend:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not active")
                        });
                    }

                    result = TaskStageStates.Suspended;
                    break;
                case CMMNTransitions.Resume:
                    if (state != TaskStageStates.Suspended)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not suspended")
                        });
                    }

                    result = TaskStageStates.Active;
                    break;
                case CMMNTransitions.Complete:
                    if (state != TaskStageStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "planitem instance is not active")
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
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "already initialized")
                        });
                    }

                    result = MilestoneEventStates.Available;
                    break;
                case CMMNTransitions.Occur:
                    if (state != MilestoneEventStates.Available)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "milestone instance is not available")
                        });
                    }

                    result = MilestoneEventStates.Completed;
                    break;
                case CMMNTransitions.Suspend:
                    if (state != MilestoneEventStates.Available)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "milestone instance is not available")
                        });
                    }

                    result = MilestoneEventStates.Suspended;
                    break;
                case CMMNTransitions.Terminate:
                    if (state != MilestoneEventStates.Available)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "milestone instance is not available")
                        });
                    }

                    result = MilestoneEventStates.Terminated;
                    break;
                case CMMNTransitions.Resume:
                    if (state != MilestoneEventStates.Suspended)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "milestone instance is not suspended")
                        });
                    }

                    result = MilestoneEventStates.Available;
                    break;
            }

            return result;
        }

        protected CaseFileItemStates? GetCaseFileItemState(CaseFileItemStates? state, CMMNTransitions transition)
        {
            CaseFileItemStates? result = null;
            switch (transition)
            {
                case CMMNTransitions.Create:
                    if (state != null)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition", "already initialized")
                        });
                    }

                    result = CaseFileItemStates.Available;
                    break;
                case CMMNTransitions.Update:
                case CMMNTransitions.Replace:
                case CMMNTransitions.RemoveChild:
                case CMMNTransitions.AddChild:
                case CMMNTransitions.AddReference:
                case CMMNTransitions.RemoveReference:
                    if (state != CaseFileItemStates.Available)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case file item is not available")
                        });
                    }

                    result = CaseFileItemStates.Available;
                    break;
                case CMMNTransitions.Delete:
                    if (state != CaseFileItemStates.Available)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case file item is not available")
                        });
                    }

                    result = CaseFileItemStates.Discarded;
                    break;
            }

            return result;
        }

        #endregion

        public abstract object Clone();

        protected void FeedCaseEltInstance(BaseCaseEltInstance elt)
        {
            elt.Id = Id;
            elt.Name = Name;
            elt.EltId = EltId;
            elt.TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>(TransitionHistories.Select(_ => (CasePlanElementInstanceTransitionHistory)_.Clone()));
        }
    }
}
