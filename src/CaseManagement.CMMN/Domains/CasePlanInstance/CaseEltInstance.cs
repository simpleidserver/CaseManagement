using CaseManagement.CMMN.Domains.CasePlanInstance;
using CaseManagement.Common.Exceptions;
using CaseManagement.Common.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public class CaseEltInstance : ICloneable, IBranchNode
    {
        public CaseEltInstance()
        {
            Children = new List<CaseEltInstance>();
            TransitionHistories = new List<CaseEltInstanceTransitionHistory>();
            Criterias = new List<Criteria>();
            Properties = new List<CaseEltInstanceProperty>();
        }

        #region Properties

        public string Id { get; set; }
        public string EltId { get; set; }
        public string Name { get; set; }
        public int NbOccurrence { get; set; }
        public MilestoneEventStates? MilestoneState { get; set; }
        public TaskStageStates? TakeStageState { get; set; }
        public CaseFileItemStates? FileState { get; set; }
        public IReadOnlyCollection<Criteria> EntryCriterions
        {
            get
            {
                return Criterias.Where(c => c.Type == CriteriaTypes.Entry).ToList();
            }
        }
        public IReadOnlyCollection<Criteria> ExitCriterions
        {
            get
            {
                return Criterias.Where(c => c.Type == CriteriaTypes.Exit).ToList();
            }
        }
        public ManualActivationRule ManualActivationRule { get; set; }
        public RepetitionRule RepetitionRule { get; set; }
        public bool IsBlocking { get; set; }
        public CasePlanElementInstanceTypes Type { get; set; }
        public ICollection<string> Incoming => EntryCriterions.SelectMany(ec => ec.SEntry.PlanItemOnParts.Select(pp => pp.SourceRef)).ToList();
        public CMMNTransitions? LatestTransition => TransitionHistories.OrderByDescending(_ => _.ExecutionDateTime).FirstOrDefault()?.Transition;
        public ICollection<CaseEltInstanceTransitionHistory> TransitionHistories { get; set; }
        public ICollection<CaseEltInstance> Children { get; set; }
        public ICollection<Criteria> Criterias { get; set; }
        public ICollection<CaseEltInstanceProperty> Properties { get; set; }

        #endregion

        #region Getters

        public bool IsTaskOrStage()
        {
            return Type == CasePlanElementInstanceTypes.STAGE ||
                Type == CasePlanElementInstanceTypes.EMPTYTASK ||
                Type == CasePlanElementInstanceTypes.HUMANTASK ||
                Type == CasePlanElementInstanceTypes.PROCESSTASK;
        }

        public bool IsMilestone()
        {
            return Type == CasePlanElementInstanceTypes.MILESTONE;
        }

        public bool IsLeaf()
        {
            return EntryCriterions == null || !EntryCriterions.Any() || EntryCriterions.All(ec => ec.SEntry == null || !ec.SEntry.PlanItemOnParts.Any());
        }

        public string GetProperty(string key)
        {
            var result = Properties.FirstOrDefault(p => p.Key == key);
            if (result == null)
            {
                return null;
            }

            return result.Value;
        }

        public void UpdateProperty(string key, string value)
        {
            var record = Properties.FirstOrDefault(p => p.Key == key);
            if (record != null)
            {
                record.Value = value;
            }
            else
            {
                Properties.Add(new CaseEltInstanceProperty
                {
                    Key = key,
                    Value = value
                });
            }
        }

        public CaseEltInstance GetParent(string id)
        {
            if (Children.Any(c => c.Id == id))
            {
                return this;
            }

            var stages = Children.Where(c => c.Type == CasePlanElementInstanceTypes.STAGE);
            foreach (var stage in stages)
            {
                var result = stage.GetParent(id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public CaseEltInstance GetChild(string id)
        {
            var child = Children.FirstOrDefault(_ => _.Id == id);
            if (child != null)
            {
                return child;
            }

            var stages = Children.Where(_ => _.Type == CasePlanElementInstanceTypes.STAGE);
            foreach (var stage in stages)
            {
                child = stage.GetChild(id);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        public List<CaseEltInstance> GetFlatListChildren()
        {
            var result = new List<CaseEltInstance> { this };
            foreach (var child in Children)
            {
                if (child.Type == CasePlanElementInstanceTypes.STAGE)
                {
                    result.AddRange(child.GetFlatListChildren());
                }
                else
                {
                    result.Add(child);
                }
            }

            return result;
        }

        public bool IsManualActivationRuleSatisfied(CasePlanInstanceExecutionContext executionContext)
        {
            return ManualActivationRule.IsSatisfied(executionContext);
        }

        #endregion

        #region Operations

        public void AddEntryCriteria(Criteria criteria)
        {
            criteria.Type = CriteriaTypes.Entry;
            Criterias.Add(criteria);
        }

        public void AddExitCriteria(Criteria criteria)
        {
            criteria.Type = CriteriaTypes.Exit;
            Criterias.Add(criteria);
        }

        public void AddChild(CaseEltInstance child)
        {
            Children.Add(child);
        }

        public void MakeTransition(CMMNTransitions transition, string message, DateTime executionDateTime)
        {
            switch(Type)
            {
                case CasePlanElementInstanceTypes.MILESTONE:
                case CasePlanElementInstanceTypes.TIMER:
                    {
                        var newState = GetMilestoneOrEventListenerState(MilestoneState, transition);
                        if (newState != null)
                        {
                            MilestoneState = newState;
                        }
                    }
                    break;
                case CasePlanElementInstanceTypes.EMPTYTASK:
                case CasePlanElementInstanceTypes.HUMANTASK:
                case CasePlanElementInstanceTypes.PROCESSTASK:
                case CasePlanElementInstanceTypes.STAGE:
                    {
                        if (transition == CMMNTransitions.ParentTerminate)
                        {
                            if (TakeStageState != TaskStageStates.Completed && TakeStageState != TaskStageStates.Terminated)
                            {
                                TakeStageState = TaskStageStates.Terminated;
                            }
                        }
                        else
                        {
                            TakeStageState = GetTaskStageState(TakeStageState, transition);
                        }
                    }
                    break;
                case CasePlanElementInstanceTypes.FILEITEM:
                    {
                        var newState = GetCaseFileItemState(FileState, transition);
                        if (newState != null)
                        {
                            FileState = newState;
                        }
                    }
                    break;
            }
            TransitionHistories.Add(new CaseEltInstanceTransitionHistory { Message = message, ExecutionDateTime = executionDateTime, Transition = transition });
        }

        public CaseEltInstance NewOccurrence(string casePlanInstanceId)
        {
            var clone = Clone() as CaseEltInstance;
            clone.NbOccurrence = NbOccurrence + 1;
            if (Type == CasePlanElementInstanceTypes.FILEITEM)
            {
                clone.Id = BuildId(casePlanInstanceId, EltId);
            }
            else
            {
                clone.Id = BuildId(casePlanInstanceId, EltId, clone.NbOccurrence);
            }

            clone.TransitionHistories = new List<CaseEltInstanceTransitionHistory>();
            clone.FileState = null;
            clone.MilestoneState = null;
            clone.TakeStageState = null;
            foreach(var criteria in clone.Criterias)
            {
                criteria.SEntry.Reset();
            }

            return clone;
        }

        #endregion

        #region Transitions

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

        public object Clone()
        {
            return new CaseEltInstance
            {
                Children = Children.Select(_ => (CaseEltInstance)_.Clone()).ToList(),
                EltId = EltId,
                Id = Id,
                Name = Name,
                TransitionHistories = TransitionHistories.Select(_ => (CaseEltInstanceTransitionHistory)_.Clone()).ToList(),
                Type = Type,
                Criterias = Criterias.Select(_ => (Criteria)_.Clone()).ToList(),
                FileState = FileState,
                IsBlocking = IsBlocking,
                ManualActivationRule = (ManualActivationRule)ManualActivationRule?.Clone(),
                MilestoneState = MilestoneState,
                NbOccurrence = NbOccurrence,
                Properties = Properties.Select(_ => (CaseEltInstanceProperty)_.Clone()).ToList(),
                RepetitionRule = (RepetitionRule)RepetitionRule?.Clone(),
                TakeStageState = TakeStageState
            };
        }

        public static string BuildId(string casePlanInstanceId, string eltId, int nbOccurrence)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{casePlanInstanceId}{eltId}{nbOccurrence}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string BuildId(string casePlanInstanceId, string eltId)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{casePlanInstanceId}{eltId}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
