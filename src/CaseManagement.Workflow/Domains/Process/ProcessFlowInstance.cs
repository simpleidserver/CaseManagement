using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstance : BaseAggregate
    {
        public ProcessFlowInstance() : base()
        {
            ExecutionSteps = new List<ProcessFlowInstanceExecutionStep>();
            Elements = new List<ProcessFlowInstanceElement>();
            Connectors = new List<ProcessFlowConnector>();
            ExecutionContext = new ProcessFlowInstanceExecutionContext();
        }

        public DateTime CreateDateTime { get; set; }
        public string ProcessFlowTemplateId { get; set; }
        public string ProcessFlowName { get; set; }
        public ProcessFlowInstanceExecutionContext ExecutionContext { get; set; }
        public ProcessFlowInstanceStatus Status { get; set; }
        public ICollection<ProcessFlowInstanceExecutionStep> ExecutionSteps { get; set; }
        public ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }

        public ICollection<ProcessFlowInstanceElement> GetRunningElements()
        {
            return Elements.Where(e => e.Status == ProcessFlowInstanceElementStatus.Started).ToList();
        }

        public ICollection<ProcessFlowInstanceElement> NextElements(string nodeId)
        {
            return Connectors.Where(c => c.Source.Id == nodeId).Select(c => c.Target).ToList();
        }

        public ICollection<ProcessFlowInstanceElement> PreviousElements(string nodeId)
        {
            return Connectors.Where(c => c.Target.Id == nodeId).Select(c => c.Source).ToList();
        }

        public ICollection<ProcessFlowInstanceElement> GetStartElements()
        {
            return Elements.Where(e => Connectors.All(c => c.Target.Id != e.Id)).ToList();
        }

        public void Launch()
        {
            var evt = new ProcessFlowInstanceLaunchedEvent(Id);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void ConfirmForm(string eltId)
        {
            DomainEvents.Add(new ProcessFlowInstanceFormConfirmedEvent(Id, eltId));
        }

        public void LaunchElement(ProcessFlowInstanceElement elt)
        {
            LaunchElement(elt.Id);
        }

        public void LaunchElement(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return;
            }

            if (elt.Status != ProcessFlowInstanceElementStatus.None)
            {
                return;
            }

            var evt = new ProcessFlowElementLaunchedEvent(Id, eltId, DateTime.UtcNow);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void CompleteElement(ProcessFlowInstanceElement elt)
        {
            CompleteElement(elt.Id);
        }

        public void CompleteElement(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return;
            }

            if (elt.Status != ProcessFlowInstanceElementStatus.Started)
            {
                return;
            }

            var evt = new ProcessFlowElementCompletedEvent(Id, eltId, DateTime.UtcNow);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void Complete()
        {
            var evt = new ProcessFlowInstanceCompletedEvent(Id);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void SetVariable(string key, int value)
        {
            SetVariable(key, value.ToString());
        }

        public void SetVariable(string key, string value)
        {
            var evt = new ProcessFlowInstanceVariableAddedEvent(Id, key, value);
            DomainEvents.Add(evt);
            Handle(evt);
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
            return int.Parse(GetVariable(key));
        }

        public static ProcessFlowInstance New(string processFlowTemplateId, string processFlowName, ICollection<ProcessFlowInstanceElement> elements, ICollection<ProcessFlowConnector> connectors)
        {
            var result = new ProcessFlowInstance();
            var evt = new ProcessFlowInstanceCreatedEvent(Guid.NewGuid().ToString(), processFlowTemplateId, processFlowName, DateTime.UtcNow, elements, connectors);
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
            if (obj is ProcessFlowInstanceCreatedEvent)
            {
                Handle((ProcessFlowInstanceCreatedEvent)obj);
            }

            if (obj is ProcessFlowInstanceLaunchedEvent)
            {
                Handle((ProcessFlowInstanceLaunchedEvent)obj);
            }

            if (obj is ProcessFlowInstanceFormConfirmedEvent)
            {
                Handle((ProcessFlowInstanceFormConfirmedEvent)obj);
            }

            if (obj is ProcessFlowInstanceCompletedEvent)
            {
                Handle((ProcessFlowInstanceCompletedEvent)obj);
            }

            if (obj is ProcessFlowElementLaunchedEvent)
            {
                Handle((ProcessFlowElementLaunchedEvent)obj);
            }

            if (obj is ProcessFlowElementCompletedEvent)
            {
                Handle((ProcessFlowElementCompletedEvent)obj);
            }

            if (obj is ProcessFlowInstanceVariableAddedEvent)
            {
                Handle((ProcessFlowInstanceVariableAddedEvent)obj);
            }
        }

        public void Handle(ProcessFlowInstanceCreatedEvent evt)
        {
            Id = evt.Id;
            ProcessFlowTemplateId = evt.ProcessFlowTemplateId;
            ProcessFlowName = evt.ProcessFlowName;
            CreateDateTime = evt.CreateDateTime;
            Elements = evt.Elements;
            Connectors = evt.Connectors;
        }

        public void Handle(ProcessFlowInstanceLaunchedEvent evt)
        {
            Status = ProcessFlowInstanceStatus.Started;
        }

        public void Handle(ProcessFlowInstanceFormConfirmedEvent evt) { }

        public void Handle(ProcessFlowInstanceCompletedEvent evt)
        {
            Status = ProcessFlowInstanceStatus.Completed;
        }
        
        public void Handle(ProcessFlowElementLaunchedEvent evt)
        {
            var elt = Elements.First(e => e.Id == evt.ProcessFlowInstanceElementId);
            ExecutionSteps.Add(new ProcessFlowInstanceExecutionStep(elt, evt.StartDateTime));
            elt.Run();
        }

        public void Handle(ProcessFlowElementCompletedEvent evt)
        {
            var executionStep = ExecutionSteps.First(e => e.Element.Id == evt.ProcessFlowInstanceElementId);
            executionStep.EndDateTime = evt.CompletedDateTime;
            executionStep.Element.Finish();
        }

        public void Handle(ProcessFlowInstanceVariableAddedEvent evt)
        {
            ExecutionContext.SetVariable(evt.Key, evt.Value);
        }

        public override object Clone()
        {
            return new ProcessFlowInstance
            {
                Id = Id,
                CreateDateTime = CreateDateTime,
                Status = Status,
                ProcessFlowTemplateId = ProcessFlowTemplateId,
                ProcessFlowName = ProcessFlowName,
                Version = Version,
                ExecutionSteps = ExecutionSteps.Select(e => (ProcessFlowInstanceExecutionStep)e.Clone()).ToList(),
                Connectors = Connectors.Select(c => (ProcessFlowConnector)c.Clone()).ToList(),
                Elements = Elements.Select(e => (ProcessFlowInstanceElement)e.Clone()).ToList(),
                ExecutionContext = (ProcessFlowInstanceExecutionContext)ExecutionContext.Clone()
            };
        }

        public string GetStreamName()
        {
            return GetStreamName(Id);
        }

        public static string GetStreamName(string id)
        {
            return $"ProcessFlowInstance_{id}";
        }
    }
}