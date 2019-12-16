using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Domains.Process.Exceptions;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class DomainEventArgs : EventArgs
    {
        public DomainEventArgs(ProcessFlowInstance processFlowInstance, DomainEvent domainEvent)
        {
            ProcessFlowInstance = processFlowInstance;
            DomainEvent = domainEvent;
        }

        public ProcessFlowInstance ProcessFlowInstance { get; set; }
        public DomainEvent DomainEvent { get; set; }
    }

    public class ProcessFlowInstance : BaseAggregate
    {
        public ProcessFlowInstance() : base()
        {
            ExecutionContext = new ProcessFlowInstanceExecutionContext();
            ExecutionSteps = new List<ProcessFlowInstanceExecutionStep>();
            Elements = new List<ProcessFlowInstanceElement>();
            Connectors = new List<ProcessFlowConnector>();
        }

        public DateTime CreateDateTime { get; set; }
        public string ProcessFlowTemplateId { get; set; }
        public string ProcessFlowName { get; set; }

        public ProcessFlowInstanceStatus? Status { get; set; }
        public ProcessFlowInstanceExecutionContext ExecutionContext { get; set; }
        public ICollection<ProcessFlowInstanceExecutionStep> ExecutionSteps { get; set; }
        public ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }
        public event EventHandler<DomainEventArgs> EventRaised;

        #region Accessors

        public bool IsFinished()
        {
            var startElts = GetStartElements();
            var result = true;
            foreach(var startElt in startElts)
            {
                if (!IsFinished(startElt))
                {
                    result = false;
                }
            }

            return result;
        }

        public FormInstanceAggregate GetFormInstance(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return null;
            }

            return elt.FormInstance;
        }

        public bool IsElementComplete(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return false;
            }

            return elt.Status == ProcessFlowInstanceElementStatus.Finished;
        }

        public bool CanStartElement(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return false;
            }

            var previousElts = PreviousElements(eltId);
            if (previousElts.Any(p => !IsElementComplete(p.Id)))
            {
                return false;
            }

            return true;
        }

        public ICollection<ProcessFlowInstanceElement> NextElements(string nodeId)
        {
            return Connectors.Where(c => c.SourceId == nodeId).SelectMany(c => Elements.Where(e => e.Id == c.TargetId)).ToList();
        }

        public ICollection<ProcessFlowInstanceElement> PreviousElements(string nodeId)
        {
            return Connectors.Where(c => c.TargetId == nodeId).SelectMany(c => Elements.Where(e => e.Id == c.SourceId)).ToList();
        }

        public ICollection<ProcessFlowInstanceElement> GetStartElements()
        {
            return Elements.Where(e => Connectors.All(c => c.TargetId != e.Id)).ToList();
        }

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

        #endregion

        #region Commands

        public void Launch()
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowInstanceLaunchedEvent(Guid.NewGuid().ToString(), Id, Version + 1);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void Cancel()
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowInstanceCanceledEvent(Guid.NewGuid().ToString(), Id, Version + 1);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void RaiseEvent(string eltId, string state)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowInstanceElementStateChangedEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, state);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void CreateForm(string eltId, string formId, string performerRef)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowElementFormCreatedEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, Guid.NewGuid().ToString(), formId, performerRef);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void ConfirmForm(string elementId, string formInstanceId, string formId, Dictionary<string, string> formContent)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowElementFormConfirmedEvent(Guid.NewGuid().ToString(), Id, Version + 1, elementId, formInstanceId, formId, formContent);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void StartElement(ProcessFlowInstanceElement elt)
        {
            StartElement(elt.Id);
        }

        public void StartElement(string eltId)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowElementStartedEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void BlockElement(ProcessFlowInstanceElement elt)
        {
            BlockElement(elt.Id);
        }

        public void BlockElement(string eltId)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowElementBlockedEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void UnblockElement(ProcessFlowInstanceElement elt)
        {
            UnblockElement(elt.Id);
        }

        public void UnblockElement(string eltId)
        {
            lock (DomainEvents)
            {
                var evt = new ProcessFlowElementUnblockedEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void CancelElement(ProcessFlowInstanceElement elt)
        {
            CancelElement(elt.Id);
        }

        public void CancelElement(string eltId)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowElementCancelledEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void InvalidElement(ProcessFlowInstanceElement elt, string errorMessage)
        {
            InvalidElement(elt.Id, errorMessage);
        }

        public void InvalidElement(string eltId, string errorMessage)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowElementInvalidEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, errorMessage, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void CompleteElement(ProcessFlowInstanceElement elt)
        {
            CompleteElement(elt.Id);
        }

        public void CompleteElement(string eltId)
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowElementCompletedEvent(Guid.NewGuid().ToString(), Id, Version + 1, eltId, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public void Complete()
        {
            lock(DomainEvents)
            {
                var evt = new ProcessFlowInstanceCompletedEvent(Guid.NewGuid().ToString(), Id, Version + 1);
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
            lock(DomainEvents)
            {
                var evt = new ProcessFlowInstanceVariableAddedEvent(Guid.NewGuid().ToString(), Id, Version + 1, key, value);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        #endregion

        public static ProcessFlowInstance New(string processFlowTemplateId, string processFlowName, ICollection<ProcessFlowInstanceElement> elements, ICollection<ProcessFlowConnector> connectors)
        {
            var result = new ProcessFlowInstance();
            var evt = new ProcessFlowInstanceCreatedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, processFlowTemplateId, processFlowName, DateTime.UtcNow, elements.Select(e => (ProcessFlowInstanceElement)e.Clone()).ToList(), connectors);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static ProcessFlowInstance New(ICollection<DomainEvent> evts)
        {
            var result = new ProcessFlowInstance();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public override void Handle(object obj)
        {
            CallHandle(obj);
        }

        public virtual void CallHandle(object obj)
        {
            if (obj is ProcessFlowInstanceCreatedEvent)
            {
                Handle((ProcessFlowInstanceCreatedEvent)obj);
            }

            if (obj is ProcessFlowInstanceLaunchedEvent)
            {
                Handle((ProcessFlowInstanceLaunchedEvent)obj);
            }

            if (obj is ProcessFlowElementFormConfirmedEvent)
            {
                Handle((ProcessFlowElementFormConfirmedEvent)obj);
            }

            if (obj is ProcessFlowInstanceCompletedEvent)
            {
                Handle((ProcessFlowInstanceCompletedEvent)obj);
            }

            if (obj is ProcessFlowElementStartedEvent)
            {
                Handle((ProcessFlowElementStartedEvent)obj);
            }

            if (obj is ProcessFlowElementCompletedEvent)
            {
                Handle((ProcessFlowElementCompletedEvent)obj);
            }

            if (obj is ProcessFlowInstanceVariableAddedEvent)
            {
                Handle((ProcessFlowInstanceVariableAddedEvent)obj);
            }

            if (obj is ProcessFlowElementLaunchedEvent)
            {
                Handle((ProcessFlowElementLaunchedEvent)obj);
            }

            if (obj is ProcessFlowInstanceElementStateChangedEvent)
            {
                Handle((ProcessFlowInstanceElementStateChangedEvent)obj);
            }

            if (obj is ProcessFlowElementInvalidEvent)
            {
                Handle((ProcessFlowElementInvalidEvent)obj);
            }

            if (obj is ProcessFlowElementBlockedEvent)
            {
                Handle((ProcessFlowElementBlockedEvent)obj);
            }

            if (obj is ProcessFlowElementFormCreatedEvent)
            {
                Handle((ProcessFlowElementFormCreatedEvent)obj);
            }

            if (obj is ProcessFlowInstanceCanceledEvent)
            {
                Handle((ProcessFlowInstanceCanceledEvent)obj);
            }

            if (obj is ProcessFlowElementCancelledEvent)
            {
                Handle((ProcessFlowElementCancelledEvent)obj);
            }

            if (obj is ProcessFlowElementUnblockedEvent)
            {
                Handle((ProcessFlowElementUnblockedEvent)obj);
            }
        }

        public void Handle(ProcessFlowInstanceCreatedEvent evt)
        {
            Id = evt.AggregateId;
            ProcessFlowTemplateId = evt.ProcessFlowTemplateId;
            ProcessFlowName = evt.ProcessFlowName;
            CreateDateTime = evt.CreateDateTime;
            Elements = evt.Elements;
            Connectors = evt.Connectors;
            Version = evt.Version;
        }

        public void Handle(ProcessFlowInstanceLaunchedEvent evt)
        {
            if (Status == ProcessFlowInstanceStatus.Started)
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "validation_error", "process instance is already launched" }
                    }
                };
            }

            Status = ProcessFlowInstanceStatus.Started;
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowInstanceCanceledEvent evt)
        {
            Status = ProcessFlowInstanceStatus.Cancelled;
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowInstanceCompletedEvent evt)
        {
            Status = ProcessFlowInstanceStatus.Completed;
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowElementFormConfirmedEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ElementId);
            foreach(var kvp in evt.FormContent)
            {
                elt.FormInstance.AddElement(new FormInstanceElement
                {
                    FormElementId = kvp.Key,
                    Value = kvp.Value
                });
            }

            elt.FormInstance.Status = FormInstanceStatus.Complete;
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowElementStartedEvent evt)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == evt.ElementId);
            if (elt == null)
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "validation_error", "process instance element doesn't exist" }
                    }
                };
            }

            elt.Status = ProcessFlowInstanceElementStatus.Launched;
            var existingElt = Elements.First(e => e.Id == evt.ElementId);
            ExecutionSteps.Add(new ProcessFlowInstanceExecutionStep(existingElt.Id, existingElt.Name, evt.StartDateTime));
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowElementCompletedEvent evt)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == evt.ElementId);
            if (elt == null)
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "validation_error", "process instance element doesn't exist" }
                    }
                };
            }

            Debug.WriteLine($"Is finished : {elt.Id}");
            elt.Status = ProcessFlowInstanceElementStatus.Finished;
            var executionStep = ExecutionSteps.First(e => e.ElementId == elt.Id);
            executionStep.EndDateTime = evt.CompletedDateTime;
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowInstanceVariableAddedEvent evt)
        {
            ExecutionContext.SetVariable(evt.Key, evt.Value);
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowInstanceElementStateChangedEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ElementId);
            elt.HandleEvent(evt.State);
            Version++;
            RaiseEvent(evt);
        }

        public void Handle(ProcessFlowElementInvalidEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ElementId);
            elt.Status = ProcessFlowInstanceElementStatus.Error;
            var executionStep = ExecutionSteps.First(e => e.ElementId == elt.Id);
            executionStep.EndDateTime = evt.InvalidDateTime;
            executionStep.ErrorMessage = evt.ErrorMessage;
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(ProcessFlowElementBlockedEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ElementId);
            elt.Status = ProcessFlowInstanceElementStatus.Blocked;
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(ProcessFlowElementUnblockedEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ElementId);
            elt.Status = ProcessFlowInstanceElementStatus.Launched;
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(ProcessFlowElementFormCreatedEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ElementId);
            var formInstance = FormInstanceAggregate.New(evt.FormInstanceId, evt.FormId);
            formInstance.RoleId = evt.PerformerRef;
            elt.SetFormInstance(formInstance);
            Version++;
            RaiseEvent(evt);
        }

        private void Handle(ProcessFlowElementCancelledEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ElementId);
            elt.Status = ProcessFlowInstanceElementStatus.Cancelled;
            var executionStep = ExecutionSteps.First(e => e.ElementId == elt.Id);
            executionStep.EndDateTime = evt.CancellationDateTime;
            Version++;
            RaiseEvent(evt);
        }

        public override object Clone()
        {
            return HandleClone();
        }

        public virtual object HandleClone()
        {
            return new ProcessFlowInstance
            {
                Id = Id,
                CreateDateTime = CreateDateTime,
                ProcessFlowTemplateId = ProcessFlowTemplateId,
                ProcessFlowName = ProcessFlowName,
                Version = Version,
                Status = Status,
                ExecutionSteps = ExecutionSteps.Select(c => (ProcessFlowInstanceExecutionStep)c.Clone()).ToList(),
                Connectors = Connectors.Select(c => (ProcessFlowConnector)c.Clone()).ToList(),
                Elements = Elements.Select(e => (ProcessFlowInstanceElement)e.Clone()).ToList(),
                ExecutionContext = (ProcessFlowInstanceExecutionContext)ExecutionContext.Clone()
            };
        }

        public virtual string GetStreamName()
        {
            return GetStreamName(Id);
        }

        public static string GetStreamName(string id)
        {
            return $"ProcessFlowInstance_{id}";
        }

        private bool IsFinished(ProcessFlowInstanceElement elt)
        {
            if (elt.Status != ProcessFlowInstanceElementStatus.Finished)
            {
                return false;
            }

            var nextElts = NextElements(elt.Id);
            var result = true;
            foreach(var nextElt in nextElts)
            {
                if (!IsFinished(nextElt))
                {
                    result = false;
                }
            }

            return result;
        }

        protected void RaiseEvent(DomainEvent evt)
        {
            if (EventRaised != null)
            {
                var clone = (ProcessFlowInstance)this.Clone();
                clone.DomainEvents.Add(evt);
                EventRaised(this, new DomainEventArgs(clone, evt));
            }
        }
    }
}