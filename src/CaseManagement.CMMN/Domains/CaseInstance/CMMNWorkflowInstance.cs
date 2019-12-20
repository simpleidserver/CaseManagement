using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.Workflow.Infrastructure;
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

    public class CMMNWorkflowInstance : BaseAggregate
    {
        public CMMNWorkflowInstance()
        {
            WorkflowElementInstances = new List<CMMNWorkflowElementInstance>();
            ExecutionContext = new CMMNWorkflowInstanceExecutionContext();
            DomainEvents = new List<DomainEvent>();
        }

        public CMMNWorkflowInstance(string id, DateTime createDateTime, string workflowDefinitionId) : base()
        {
            Id = id;
            CreateDateTime = createDateTime;
            WorkflowDefinitionId = workflowDefinitionId;
        }

        public string WorkflowDefinitionId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public CMMNWorkflowInstanceExecutionContext ExecutionContext { get; set; }
        public ICollection<CMMNWorkflowElementInstance> WorkflowElementInstances { get; set; }
        public event EventHandler<DomainEventArgs> EventRaised;

        #region Get

        public bool ContainsVariable(string key)
        {
            return ExecutionContext.ContainsVariable(key);
        }

        public string GetVariable(string key)
        {
            return ExecutionContext.GetVariable(key);
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
        
        public ICollection<CMMNCriterion> GetEntryCriterions(string id, CMMNWorkflowDefinition workflowDefinition)
        {
            var planItemDefinition = GetWorkflowElementDefinition(id, workflowDefinition);
            if (planItemDefinition == null)
            {
                return new List<CMMNCriterion>();
            }

            return planItemDefinition.EntryCriterions;
        }

        public bool IsRepetitionRuleSatisfied(string planItemId, CMMNWorkflowDefinition workflowDefinition, bool listenEvent = false)
        {
            var planItemDef = workflowDefinition.Elements.First(p => p.Id == planItemId);
            if (planItemDef == null || planItemDef.ActivationRule != CMMNActivationRuleTypes.Repetition || planItemDef.RepetitionRule == null)
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

        public bool IsManualActivationRuleSatisfied(string id, CMMNWorkflowDefinition workflowDefinition)
        {
            var planItemDef = GetWorkflowElementDefinition(id, workflowDefinition);
            if (planItemDef == null || planItemDef.ActivationRule != CMMNActivationRuleTypes.ManualActivation || planItemDef.ManualActivationRule == null)
            {
                return false;
            }

            if (planItemDef.ManualActivationRule.Expression != null && !ExpressionParser.IsValid(planItemDef.ManualActivationRule.Expression.Body, this))
            {
                return false;
            }

            return true;
        }

        public bool IsCriteriaSatisfied(CMMNCriterion criteria, int version)
        {
            var planItemOnParts = criteria.SEntry.PlanItemOnParts;
            foreach (var planItemOnPart in planItemOnParts)
            {
                var source = WorkflowElementInstances.FirstOrDefault(p => p.Version == version && p.WorkflowElementDefinitionId == planItemOnPart.SourceRef);
                if (source == null)
                {
                    return false;
                }

                if (!source.TransitionHistories.Any(t => t.Transition == planItemOnPart.StandardEvent))
                {
                    return false;
                }
            }

            return true;
        }

        public CMMNWorkflowElementDefinition GetWorkflowElementDefinition(string id, CMMNWorkflowDefinition workflowDefinition)
        {
            var elementInstance = WorkflowElementInstances.FirstOrDefault(p => p.Id == id);
            if (elementInstance == null)
            {
                return null;
            }

            return workflowDefinition.Elements.FirstOrDefault(p => p.Id == elementInstance.WorkflowElementDefinitionId);
        }

        public CMMNWorkflowElementInstance GetWorkflowElementInstance(string id)
        {
            return WorkflowElementInstances.FirstOrDefault(p => p.Id == id);
        }

        public CMMNWorkflowElementInstance GetWorkflowElementInstance(string workflowItemDefinitionId, int version)
        {
            return WorkflowElementInstances.FirstOrDefault(p => p.WorkflowElementDefinitionId == workflowItemDefinitionId && p.Version == version);
        }

        public CMMNWorkflowElementInstance GetLastWorkflowElementInstance(string workflowItemDefinitionId)
        {
            var result = WorkflowElementInstances.Where(p => p.WorkflowElementDefinitionId == workflowItemDefinitionId).OrderByDescending(p => p.Version).FirstOrDefault();
            return result;
        }

        public string GetStreamName()
        {
            return GetStreamName(Id);
        }

        #endregion

        #region Commands
        
        public CMMNWorkflowElementInstance CreateWorkflowElementInstance(CMMNWorkflowElementDefinition workflowElementDefinition)
        {
            return CreateWorkflowElementInstance(workflowElementDefinition.Id, workflowElementDefinition.Type);
        }

        public CMMNWorkflowElementInstance CreateWorkflowElementInstance(string planItemDefinitionId, CMMNWorkflowElementTypes workflowElementType)
        {
            lock (DomainEvents)
            {
                var evt = new CMMNWorkflowElementCreatedEvent(Guid.NewGuid().ToString(), Id, Version + 1, Guid.NewGuid().ToString(), planItemDefinitionId, workflowElementType, DateTime.UtcNow);
                var result = Handle(evt);
                DomainEvents.Add(evt);
                return result;
            }
        }

        public void CreateFormInstance(string elementId, string formId, string performerRef)
        {
            lock (DomainEvents)
            {
                var evt = new CMMNWorkflowElementInstanceFormCreatedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, Guid.NewGuid().ToString(), formId, performerRef);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void MakeTransition(string elementId, CMMNTransitions transition)
        {
            lock(DomainEvents)
            {
                var evt = new CMMNWorkflowElementTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, transition, DateTime.UtcNow);
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
                var evt = new CMMNWorkflowInstanceVariableAddedEvent(Guid.NewGuid().ToString(), Id, Version + 1, key, value);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        #endregion

        public static CMMNWorkflowInstance New(CMMNWorkflowDefinition workflowDefinition)
        {
            var result = new CMMNWorkflowInstance();
            var evt = new CMMNWorkflowInstanceCreatedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, workflowDefinition.Id, DateTime.UtcNow);
            result.DomainEvents.Add(evt);
            result.Handle(evt);
            return result;
        }

        public static string GetStreamName(string id)
        {
            return $"cmmn-workflow-instance-{id}";
        }

        #region Handle events

        public override void Handle(object obj)
        {
            if (obj is CMMNWorkflowInstanceCreatedEvent)
            {
                Handle((CMMNWorkflowInstanceCreatedEvent)obj);
            }

            if (obj is CMMNWorkflowElementCreatedEvent)
            {
                Handle((CMMNWorkflowElementCreatedEvent)obj);
            }

            if (obj is CMMNWorkflowElementTransitionRaisedEvent)
            {
                Handle((CMMNWorkflowElementTransitionRaisedEvent)obj);
            }

            if (obj is CMMNWorkflowInstanceVariableAddedEvent)
            {
                Handle((CMMNWorkflowInstanceVariableAddedEvent)obj);
            }

            if (obj is CMMNWorkflowElementInstanceFormCreatedEvent)
            {
                Handle((CMMNWorkflowElementInstanceFormCreatedEvent)obj);
            }
        }

        private void Handle(CMMNWorkflowInstanceCreatedEvent evt)
        {
            Id = evt.Id;
            CreateDateTime = evt.CreateDateTime;
            WorkflowDefinitionId = evt.DefinitionId;
        }

        private CMMNWorkflowElementInstance Handle(CMMNWorkflowElementCreatedEvent evt)
        {
            var existingPlanItem = WorkflowElementInstances.Where(p => p.WorkflowElementDefinitionId == evt.WorkflowElementDefinitionId).OrderByDescending(p => p.Version).FirstOrDefault();
            var elt = new CMMNWorkflowElementInstance(evt.ElementId, evt.CreateDateTime, evt.WorkflowElementDefinitionId, evt.WorkflowElementDefinitionType, existingPlanItem == null ? 0 : existingPlanItem.Version + 1);
            WorkflowElementInstances.Add(elt);
            elt.UpdateState(CMMNTransitions.Create, evt.CreateDateTime);
            Version++;
            RaiseEvent(evt);
            return elt;
        }

        private void Handle(CMMNWorkflowElementTransitionRaisedEvent evt)
        {
            var elt = WorkflowElementInstances.First(p => p.Id == evt.ElementId);
            elt.UpdateState(evt.Transition, evt.UpdateDateTime);
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(CMMNWorkflowInstanceVariableAddedEvent evt)
        {
            ExecutionContext.SetVariable(evt.Key, evt.Value);
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(CMMNWorkflowElementInstanceFormCreatedEvent evt)
        {
            var planItemInstance = WorkflowElementInstances.First(p => p.Id == evt.ElementId);
            planItemInstance.FormInstanceId = evt.FormInstanceId;
            Version++;
            RaiseEvent(evt);
        }

        #endregion

        public override object Clone()
        {
            return new CMMNWorkflowInstance(Id, CreateDateTime, null);
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