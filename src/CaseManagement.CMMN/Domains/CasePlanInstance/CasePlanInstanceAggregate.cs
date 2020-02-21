using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class DomainEventArgs
    {
        public DomainEventArgs(DomainEvent domainEvt)
        {
            DomainEvt = domainEvt;
        }

        public DomainEvent DomainEvt { get; set; }
    }

    public class CasePlanInstanceAggregate : BaseAggregate
    {
        public CasePlanInstanceAggregate()
        {
            ExecutionContext = new CaseInstanceExecutionContext();
            StateHistories = new List<CaseInstanceHistory>();
            TransitionHistories = new List<CaseInstanceTransitionHistory>();
            ExecutionHistories = new List<CaseElementExecutionHistory>();
            WorkflowElementInstances = new List<CaseElementInstance>();
            DomainEvents = new List<DomainEvent>();
            Roles = new List<string>();
        }

        public CasePlanInstanceAggregate(string id, DateTime createDateTime, string casePlanId) : base()
        {
            Id = id;
            CreateDateTime = createDateTime;
            CasePlanId = casePlanId;
        }
        
        public string CasePlanId { get; set; }
        public string CaseOwner { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string State { get; set; }
        public CaseInstanceExecutionContext ExecutionContext { get; set; }
        public ICollection<CaseInstanceHistory> StateHistories { get; set; }
        public ICollection<CaseInstanceTransitionHistory> TransitionHistories { get; set; }
        public ICollection<CaseElementExecutionHistory> ExecutionHistories { get; set; }
        public ICollection<CaseElementInstance> WorkflowElementInstances { get; set; }
        public ICollection<string> Roles { get; set; }
        public event EventHandler<DomainEventArgs> EventRaised;

        #region Get

        public bool ContainsVariable(string key)
        {
            lock (ExecutionContext)
            {
                return ExecutionContext.ContainsVariable(key);
            }
        }

        public string GetVariable(string key)
        {
            lock (ExecutionContext)
            {
                return ExecutionContext.GetVariable(key);
            }
        }

        public int GetNumberVariable(string key)
        {
            var value = GetVariable(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return default(int);
            }

            return int.Parse(GetVariable(key));
        }

        public ICollection<Criteria> GetEntryCriterions(string id, CasePlanAggregate workflowDefinition)
        {
            var planItemDefinition = GetWorkflowElementDefinition(id, workflowDefinition);
            if (planItemDefinition == null)
            {
                return new List<Criteria>();
            }

            return planItemDefinition.EntryCriterions;
        }

        public bool IsRepetitionRuleSatisfied(string planItemId, CasePlanAggregate workflowDefinition, bool listenEvent = false)
        {
            var planItemDef = workflowDefinition.GetElement(planItemId);
            if (planItemDef == null || planItemDef.ActivationRule != ActivationRuleTypes.Repetition || planItemDef.RepetitionRule == null)
            {
                return false;
            }

            if (planItemDef.RepetitionRule.Condition != null && !ExpressionParser.IsValid(planItemDef.RepetitionRule.Condition.Body, this))
            {
                return false;
            }

            if (!listenEvent)
            {
                if (!planItemDef.EntryCriterions.Any())
                {
                    return true;
                }

                return false;
            }

            if (!planItemDef.EntryCriterions.Any())
            {
                return false;
            }

            var lastPlanItem = GetLastWorkflowElementInstance(planItemId);
            if (!planItemDef.EntryCriterions.Any(c => IsCriteriaSatisfied(c, lastPlanItem.Version + 1)))
            {
                return false;
            }

            return true;
        }

        public bool IsManualActivationRuleSatisfied(string id, CasePlanAggregate workflowDefinition)
        {
            var planItemDef = GetWorkflowElementDefinition(id, workflowDefinition);
            if (planItemDef == null || planItemDef.ActivationRule != ActivationRuleTypes.ManualActivation || planItemDef.ManualActivationRule == null)
            {
                return false;
            }

            if (planItemDef.ManualActivationRule.Expression != null && !ExpressionParser.IsValid(planItemDef.ManualActivationRule.Expression.Body, this))
            {
                return false;
            }

            return true;
        }

        public bool IsCriteriaSatisfied(Criteria criteria, int version)
        {
            var planItemOnParts = criteria.SEntry.PlanItemOnParts;
            var caseItemOnParts = criteria.SEntry.FileItemOnParts;
            lock (WorkflowElementInstances)
            {
                foreach (var planItemOnPart in planItemOnParts)
                {
                    var source = WorkflowElementInstances.FirstOrDefault(p => p.Version == version && p.CaseElementDefinitionId == planItemOnPart.SourceRef);
                    if (source == null)
                    {
                        return false;
                    }

                    var transitionHistories = source.TransitionHistories.ToList();
                    if (!transitionHistories.Any(t => t.Transition == planItemOnPart.StandardEvent))
                    {
                        return false;
                    }
                }

                foreach (var caseItemOnPart in caseItemOnParts)
                {
                    var source = WorkflowElementInstances.FirstOrDefault(p => p.Version == version && p.CaseElementDefinitionId == caseItemOnPart.SourceRef);
                    if (source == null)
                    {
                        return false;
                    }

                    var transitionHistories = source.TransitionHistories.ToList();
                    if (!transitionHistories.Any(t => t.Transition == caseItemOnPart.StandardEvent))
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
        }

        public bool IsWorkflowElementDefinitionFinished(string elementDefinitionId)
        {
            lock (ExecutionHistories)
            {
                var executionHistory = ExecutionHistories.FirstOrDefault(e => e.CaseElementDefinitionId == elementDefinitionId);
                if (executionHistory == null || executionHistory.EndDateTime == null)
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsWorkflowElementDefinitionFailed(string elementDefinitionId)
        {
            var last = GetLastWorkflowElementInstance(elementDefinitionId);
            if (last != null && last.State == Enum.GetName(typeof(TaskStates), TaskStates.Failed))
            {
                return true;
            }

            return false;
        }

        public CasePlanElement GetWorkflowElementDefinition(string id, CasePlanAggregate workflowDefinition)
        {
            lock (WorkflowElementInstances)
            {
                var elementInstance = WorkflowElementInstances.FirstOrDefault(p => p.Id == id);
                if (elementInstance == null)
                {
                    return null;
                }

                return workflowDefinition.GetElement(elementInstance.CaseElementDefinitionId);
            }
        }

        public CaseElementInstance GetWorkflowElementInstance(string id)
        {
            lock (WorkflowElementInstances)
            {
                return WorkflowElementInstances.FirstOrDefault(p => p.Id == id);
            }
        }

        public CaseElementInstance GetWorkflowElementInstance(string workflowItemDefinitionId, int version)
        {
            lock (WorkflowElementInstances)
            {
                return WorkflowElementInstances.FirstOrDefault(p => p.CaseElementDefinitionId == workflowItemDefinitionId && p.Version == version);
            }
        }

        public ICollection<CaseElementInstance> GetWorkflowElementInstancesByParentId(string parentId)
        {
            lock (WorkflowElementInstances)
            {
                return WorkflowElementInstances.Where(e => e.ParentId == parentId).ToList();
            }
        }

        public CaseElementInstance GetLastWorkflowElementInstance(string workflowItemDefinitionId)
        {
            lock (WorkflowElementInstances)
            {
                var result = WorkflowElementInstances.Where(p => p.CaseElementDefinitionId == workflowItemDefinitionId).OrderByDescending(p => p.Version).FirstOrDefault();
                return result;
            }
        }

        public string GetStreamName()
        {
            return GetStreamName(Id);
        }

        public bool IsRunning()
        {
            return State == Enum.GetName(typeof(CaseStates), CaseStates.Active) || State == Enum.GetName(typeof(CaseStates), CaseStates.Suspended);
        }
       
        #endregion

        #region Commands

        public CaseElementInstance CreateWorkflowElementInstance(CasePlanElement workflowElementDefinition, string parentId = null)
        {
            return CreateWorkflowElementInstance(workflowElementDefinition.Id, workflowElementDefinition.Type, parentId);
        }

        public CaseElementInstance CreateWorkflowElementInstance(string planItemDefinitionId, CaseElementTypes workflowElementType, string parentId = null)
        {
            lock (DomainEvents)
            {
                var evt = new CaseElementCreatedEvent(Guid.NewGuid().ToString(), Id, Version + 1, Guid.NewGuid().ToString(), planItemDefinitionId, workflowElementType, DateTime.UtcNow, parentId);
                var result = Handle(evt);
                DomainEvents.Add(evt);
                return result;
            }
        }

        public void StartElement(string elementDefinitionId)
        {
            lock (DomainEvents)
            {
                var evt = new CaseElementStartedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementDefinitionId, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void FinishElement(string elementDefinitionId)
        {
            lock (DomainEvents)
            {
                var evt = new CaseElementFinishedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementDefinitionId, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransition(CMMNTransitions transition)
        {
            lock (DomainEvents)
            {
                var evt = new CaseTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, transition, DateTime.UtcNow);
                Handle(evt);
                if (transition == CMMNTransitions.Reactivate)
                {
                    foreach (var elt in WorkflowElementInstances)
                    {
                        if (elt.IsFail())
                        {
                            MakeTransition(elt.Id, CMMNTransitions.Reactivate);
                        }
                    }
                }

                DomainEvents.Add(evt);
            }
        }

        public void MakeTransition(string elementId, CMMNTransitions transition)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.First(e => e.Id == elementId);
                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, transition, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionEnable(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsAvailable())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Enable, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionAddChild(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsAvailable())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.AddChild, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionStart(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsAvailable())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Start, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionFault(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsActive())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Fault, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionOccur(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsAvailable())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Occur, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionReactivate(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsFail())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Reactivate, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionParentSuspend(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsActive())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.ParentSuspend, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionSuspend(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsActive())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Suspend, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionParentResume(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsSuspend())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.ParentResume, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionResume(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsSuspend())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Resume, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionParentTerminate(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.CanBeTerminated())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.ParentTerminate, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionTerminate(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsActive())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Terminate, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransitionComplete(string elementId)
        {
            lock (DomainEvents)
            {
                var elt = WorkflowElementInstances.FirstOrDefault(e => e.Id == elementId);
                if (elt == null)
                {
                    return;
                }

                if (!elt.IsActive())
                {
                    return;
                }

                var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, elt.CaseElementDefinitionId, CMMNTransitions.Complete, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void SetVariable(string key, int value)
        {
            SetVariable(key, value.ToString());
        }

        public void SetVariable(string key, string value)
        {
            lock (DomainEvents)
            {
                var evt = new CaseInstanceVariableAddedEvent(Guid.NewGuid().ToString(), Id, Version + 1, key, value);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        #endregion

        public static CasePlanInstanceAggregate New(CasePlanAggregate workflowDefinition)
        {
            var result = new CasePlanInstanceAggregate();
            var evt = new CaseInstanceCreatedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, workflowDefinition.Id, DateTime.UtcNow, workflowDefinition.CaseOwner, workflowDefinition.Roles);
            var secondEvt = new CaseTransitionRaisedEvent(Guid.NewGuid().ToString(), evt.AggregateId, 1, CMMNTransitions.Create, DateTime.UtcNow);
            result.DomainEvents.Add(evt);
            result.DomainEvents.Add(secondEvt);
            result.Handle(evt);
            result.Handle(secondEvt);
            return result;
        }

        public static CasePlanInstanceAggregate New(List<DomainEvent> evts)
        {
            var result = new CasePlanInstanceAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }
        
        public static string GetStreamName(string id)
        {
            return $"cmmn-workflow-instance-{id}";
        }

        #region Handle events

        public override void Handle(object obj)
        {
            if (obj is CaseInstanceCreatedEvent)
            {
                Handle((CaseInstanceCreatedEvent)obj);
            }

            if (obj is CaseElementCreatedEvent)
            {
                Handle((CaseElementCreatedEvent)obj);
            }

            if (obj is CaseElementTransitionRaisedEvent)
            {
                Handle((CaseElementTransitionRaisedEvent)obj);
            }

            if (obj is CaseInstanceVariableAddedEvent)
            {
                Handle((CaseInstanceVariableAddedEvent)obj);
            }

            if (obj is CaseElementStartedEvent)
            {
                Handle((CaseElementStartedEvent)obj);
            }

            if (obj is CaseElementFinishedEvent)
            {
                Handle((CaseElementFinishedEvent)obj);
            }

            if (obj is CaseTransitionRaisedEvent)
            {
                Handle((CaseTransitionRaisedEvent)obj);
            }
        }

        private void Handle(CaseInstanceCreatedEvent evt)
        {
            Id = evt.AggregateId;
            CreateDateTime = evt.CreateDateTime;
            CasePlanId = evt.CaseDefinitionId;
            CaseOwner = evt.Performer;
            Roles = evt.Roles;
        }

        private CaseElementInstance Handle(CaseElementCreatedEvent evt)
        {
            var existingPlanItem = WorkflowElementInstances.Where(p => p.CaseElementDefinitionId == evt.CaseElementDefinitionId).OrderByDescending(p => p.Version).FirstOrDefault();
            var elt = new CaseElementInstance(evt.CaseElementId, evt.CreateDateTime, evt.CaseElementDefinitionId, evt.CaseElementDefinitionType, existingPlanItem == null ? 0 : existingPlanItem.Version + 1, evt.ParentId);
            WorkflowElementInstances.Add(elt);
            elt.UpdateState(CMMNTransitions.Create, evt.CreateDateTime);
            Version++;
            RaiseEvent(evt);
            return elt;
        }

        private void Handle(CaseElementTransitionRaisedEvent evt)
        {
            var elt = WorkflowElementInstances.First(p => p.Id == evt.CaseElementId);
            elt.UpdateState(evt.Transition, evt.UpdateDateTime);
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(CaseInstanceVariableAddedEvent evt)
        {
            ExecutionContext.SetVariable(evt.Key, evt.Value);
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(CaseElementStartedEvent evt)
        {
            ExecutionHistories.Add(new CaseElementExecutionHistory(evt.CaseElementDefinitionId, evt.StartDateTime));
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(CaseElementFinishedEvent evt)
        {
            var executionHistory = ExecutionHistories.First(e => e.CaseElementDefinitionId == evt.CaseElementDefinitionId);
            executionHistory.EndDateTime = evt.EndDateTime;
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(CaseTransitionRaisedEvent evt)
        {
            CaseStates? state = null;
            switch (evt.Transition)
            {
                case CMMNTransitions.Create:
                    if (!string.IsNullOrWhiteSpace(State))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is already initialized" }
                        });
                    }

                    state = CaseStates.Active;
                    break;
                case CMMNTransitions.Complete:
                    if (State != Enum.GetName(typeof(CaseStates), CaseStates.Active))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    state = CaseStates.Completed;
                    break;
                case CMMNTransitions.Terminate:
                    if (State != Enum.GetName(typeof(CaseStates), CaseStates.Active))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    state = CaseStates.Terminated;
                    break;
                case CMMNTransitions.Fault:
                    if (State != Enum.GetName(typeof(CaseStates), CaseStates.Active))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    state = CaseStates.Failed;
                    break;
                case CMMNTransitions.Suspend:
                    if (State != Enum.GetName(typeof(CaseStates), CaseStates.Active))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not active" }
                        });
                    }

                    state = CaseStates.Suspended;
                    break;
                case CMMNTransitions.Resume:
                    if (State != Enum.GetName(typeof(CaseStates), CaseStates.Suspended))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not suspend" }
                        });
                    }

                    state = CaseStates.Active;
                    break;
                case CMMNTransitions.Close:
                    if (State != Enum.GetName(typeof(CaseStates), CaseStates.Completed) &&
                        State != Enum.GetName(typeof(CaseStates), CaseStates.Terminated) &&
                        State != Enum.GetName(typeof(CaseStates), CaseStates.Failed) &&
                        State != Enum.GetName(typeof(CaseStates), CaseStates.Suspended))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not completed / terminated / failed / suspended" }
                        });
                    }

                    state = CaseStates.Closed;
                    break;
                case CMMNTransitions.Reactivate:
                    if (State != Enum.GetName(typeof(CaseStates), CaseStates.Completed) && 
                        State != Enum.GetName(typeof(CaseStates), CaseStates.Terminated) &&
                        State != Enum.GetName(typeof(CaseStates), CaseStates.Failed) &&
                        State != Enum.GetName(typeof(CaseStates), CaseStates.Suspended))
                    {
                        throw new AggregateValidationException(new Dictionary<string, string>
                        {
                            { "transition", "case instance is not completed / terminated / failed / suspended" }
                        });
                    }

                    state = CaseStates.Active;
                    break;
            }

            if (state != null)
            {
                State = Enum.GetName(typeof(CaseStates), state);
                StateHistories.Add(new CaseInstanceHistory(State, DateTime.UtcNow));
                TransitionHistories.Add(new CaseInstanceTransitionHistory(evt.Transition, DateTime.UtcNow));
                Version++;
                RaiseEvent(evt);
            }
        }

        #endregion

        public override bool Equals(object obj)
        {
            var workflowInstance = obj as CasePlanInstanceAggregate;
            if (workflowInstance == null)
            {
                return false;
            }

            return workflowInstance.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override object Clone()
        {
            return new CasePlanInstanceAggregate(Id, CreateDateTime, CasePlanId)
            {
                ExecutionContext = ExecutionContext == null ? null : (CaseInstanceExecutionContext)ExecutionContext.Clone(),
                ExecutionHistories = ExecutionHistories == null ? null : ExecutionHistories.Select(e => (CaseElementExecutionHistory)e.Clone()).ToList(),
                State = State,
                StateHistories = StateHistories == null ? null : StateHistories.Select(s => (CaseInstanceHistory)s.Clone()).ToList(),
                TransitionHistories = TransitionHistories == null ? null : TransitionHistories.Select(t => (CaseInstanceTransitionHistory)t.Clone()).ToList(),
                Version = Version,
                CasePlanId = CasePlanId,
                WorkflowElementInstances = WorkflowElementInstances == null ? null : WorkflowElementInstances.Select(w => (CaseElementInstance)w.Clone()).ToList(),
                CaseOwner = CaseOwner,
                Roles = Roles.ToList()
            };
        }

        private void RaiseEvent(DomainEvent evt)
        {
            if (EventRaised != null)
            {
                EventRaised(this, new DomainEventArgs(evt));
            }
        }
    }
}