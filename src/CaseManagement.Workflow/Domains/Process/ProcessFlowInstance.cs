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
            Elements = new List<ProcessFlowInstanceElement>();
            Connectors = new List<ProcessFlowConnector>();
        }

        public DateTime CreateDateTime { get; set; }
        public ProcessFlowInstanceStatus Status { get; set; }
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

        public void Complete()
        {
            var evt = new ProcessFlowInstanceCompletedEvent(Id);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public static ProcessFlowInstance New(ICollection<ProcessFlowInstanceElement> elements, ICollection<ProcessFlowConnector> connectors)
        {
            var result = new ProcessFlowInstance();
            var evt = new ProcessFlowInstanceCreatedEvent(Guid.NewGuid().ToString(), DateTime.UtcNow, elements, connectors);
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
        }

        public void Handle(ProcessFlowInstanceCreatedEvent evt)
        {
            Id = evt.Id;
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

        public override object Clone()
        {
            return new ProcessFlowInstance
            {
                Id = Id,
                CreateDateTime = CreateDateTime,
                Status = Status,
                Version = Version,
                Connectors = Connectors.Select(c => (ProcessFlowConnector)c.Clone()).ToList(),
                Elements = Elements.Select(e => (ProcessFlowInstanceElement)e.Clone()).ToList()
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