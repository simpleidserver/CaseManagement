using CaseManagement.CMMN.Domains.Events;
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
            PlanItemInstances = new List<CMMNPlanItemInstance>();
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
        public ICollection<CMMNPlanItemInstance> PlanItemInstances { get; set; }
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

        public CMMNPlanItemInstance GetPlanItemInstance(string id)
        {
            return PlanItemInstances.FirstOrDefault(p => p.Id == id);
        }

        public CMMNPlanItemInstance GetPlanItemInstance(string planItemDefinitionId, int version)
        {
            return PlanItemInstances.FirstOrDefault(p => p.PlanItemDefinitionId == planItemDefinitionId && p.Version == version);
        }

        public string GetStreamName()
        {
            return GetStreamName(Id);
        }

        #endregion

        #region Commands

        public CMMNPlanItemInstance CreatePlanItemInstance(CMMNPlanItemDefinition planItemDefinition)
        {
            lock(DomainEvents)
            {
                var evt = new CMMNPlanItemInstanceCreatedEvent(Guid.NewGuid().ToString(), Id, Version + 1, Guid.NewGuid().ToString(), planItemDefinition.Id, planItemDefinition.PlanItemDefinitionType, DateTime.UtcNow);
                var result = Handle(evt);
                DomainEvents.Add(evt);
                return result;
            }
        }

        public void MakeTransition(string elementId, CMMNPlanItemTransitions transition)
        {
            lock(DomainEvents)
            {
                var evt = new CMMNPlanItemTransitionRaisedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, transition, DateTime.UtcNow);
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

            if (obj is CMMNPlanItemInstanceCreatedEvent)
            {
                Handle((CMMNPlanItemInstanceCreatedEvent)obj);
            }

            if (obj is CMMNPlanItemTransitionRaisedEvent)
            {
                Handle((CMMNPlanItemTransitionRaisedEvent)obj);
            }

            if (obj is CMMNWorkflowInstanceVariableAddedEvent)
            {
                Handle((CMMNWorkflowInstanceVariableAddedEvent)obj);
            }
        }

        private void Handle(CMMNWorkflowInstanceCreatedEvent evt)
        {
            Id = evt.Id;
            CreateDateTime = evt.CreateDateTime;
            WorkflowDefinitionId = evt.DefinitionId;
        }

        private CMMNPlanItemInstance Handle(CMMNPlanItemInstanceCreatedEvent evt)
        {
            var elt = new CMMNPlanItemInstance(evt.ElementId, evt.CreateDateTime, evt.PlanItemDefinitionId, evt.PlanItemDefinitionType);
            PlanItemInstances.Add(elt);
            elt.UpdateState(CMMNPlanItemTransitions.Create, evt.CreateDateTime);
            Version++;
            RaiseEvent(evt);
            return elt;
        }

        private void Handle(CMMNPlanItemTransitionRaisedEvent evt)
        {
            var elt = PlanItemInstances.First(p => p.Id == evt.ElementId);
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