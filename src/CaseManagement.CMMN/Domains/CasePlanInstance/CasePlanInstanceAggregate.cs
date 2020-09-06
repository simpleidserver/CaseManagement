using CaseManagement.CMMN.Domains.CasePlanInstance.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CasePlanInstanceAggregate : BaseAggregate
    {
        public CasePlanInstanceAggregate()
        {
            Roles = new List<CasePlanInstanceRole>();
        }

        public string Name { get; set; }
        public CaseStates? State { get; set; }
        public ICollection<CasePlanInstanceRole> Roles { get; set; }
        public StageElementInstance Content { get; set; }
        public DateTime CreateDateTime { get; set; }

        public bool IsEntryCriteriaSatisfied(string id)
        {
            var child = GetChild(id);
            return child.EntryCriterions == null || !child.EntryCriterions.Any() || child.EntryCriterions.Any(_ => IsCriteriaSatisfied(_));
        }

        public bool IsCriteriaSatisfied(Criteria criteria)
        {
            Func<string, CMMNTransitions, bool> callback = (sourceRef, standardEvent) =>
            {
                var source = GetChild(sourceRef);
                if (standardEvent != source.LatestTransition)
                {
                    return false;
                }

                return true;
            };
            foreach(var planItemOnPart in criteria.SEntry.PlanItemOnParts)
            {
                if (!callback(planItemOnPart.SourceRef, planItemOnPart.StandardEvent))
                {
                    return false;
                }
            }

            foreach(var caseItemOnPart in criteria.SEntry.FileItemOnParts)
            {
                if (!callback(caseItemOnPart.SourceRef, caseItemOnPart.StandardEvent))
                {
                    return false;
                }
            }

            if (criteria.SEntry.IfPart != null && criteria.SEntry.IfPart.Condition != null)
            {
                if (!ExpressionParser.IsValid(criteria.SEntry.IfPart.Condition, this))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsManualActivationRuleSatisfied()
        {
            return false;
        }

        public CasePlanElementInstance GetChild(string id)
        {
            if (Content.Id == id)
            {
                return Content;
            }

            var child = Content.Children.FirstOrDefault(_ => _.Id == id);
            return child;
        }

        public static CasePlanInstanceAggregate New(string id, CasePlanAggregate caseplan)
        {
            var result = new CasePlanInstanceAggregate();
            var roles = caseplan.Roles.Select(_ => new CasePlanInstanceRole
            {
                Id = _.Id,
                Name = _.Name
            });
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, roles, caseplan.XmlContent, DateTime.UtcNow);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        #region Operations

        public void MakeTransition(CMMNTransitions transition)
        {
            var evt = new CaseTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, transition, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void MakeTransition(CasePlanElementInstance element, CMMNTransitions transition)
        {
            MakeTransition(element.Id, transition);
        }

        public void MakeTransition(string elementId, CMMNTransitions transition)
        {
            var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, transition, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        #endregion

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        #region Handle domain events

        public override void Handle(dynamic obj)
        {
            Handle(obj);
        }

        private void Handle(CaseElementTransitionRaisedEvent evt)
        {
            var child = GetChild(evt.ElementId);
            child.MakeTransition(evt.Transition, DateTime.UtcNow);
        }

        private void Handle(CasePlanInstanceCreatedEvent evt)
        {
            Roles = evt.Roles.ToList();
            Content = CMMNParser.ExtractStage(evt.XmlContent);
            CreateDateTime = evt.CreateDateTime;
        }

        private void Handle(CaseTransitionRaisedEvent evt)
        {
            switch (evt.Transition)
            {
                case CMMNTransitions.Create:
                    if (State != null)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is already initialized" }
                        });
                    }

                    State = CaseStates.Active;
                    break;
                case CMMNTransitions.Complete:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    State = CaseStates.Completed;
                    break;
                case CMMNTransitions.Terminate:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    State = CaseStates.Terminated;
                    break;
                case CMMNTransitions.Fault:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    State = CaseStates.Failed;
                    break;
                case CMMNTransitions.Suspend:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    State = CaseStates.Suspended;
                    break;
                case CMMNTransitions.Resume:
                    if (State != CaseStates.Suspended)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not suspend" }
                        });
                    }

                    State = CaseStates.Active;
                    break;
                case CMMNTransitions.Close:
                    if (State != CaseStates.Completed &&
                        State != CaseStates.Terminated &&
                        State != CaseStates.Failed &&
                        State != CaseStates.Suspended)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not completed / terminated / failed / suspended" }
                        });
                    }

                    State = CaseStates.Closed;
                    break;
                case CMMNTransitions.Reactivate:
                    if (State != CaseStates.Completed &&
                        State != CaseStates.Terminated &&
                        State != CaseStates.Failed &&
                        State != CaseStates.Suspended)
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not completed / terminated / failed / suspended" }
                        });
                    }

                    State = CaseStates.Active;
                    break;
            }
        }

        #endregion
    }
}