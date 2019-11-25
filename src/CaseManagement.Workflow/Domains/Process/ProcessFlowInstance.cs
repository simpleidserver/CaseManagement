using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstance
    {
        private ProcessFlowInstance()
        {
            Elements = new List<ProcessFlowInstanceElement>();
            Connectors = new List<ProcessFlowConnector>();
        }

        private ProcessFlowInstance(string id) : this()
        {
            Id = id;
        }

        private ProcessFlowInstance(string id, DateTime createDateTime) : this(id)
        {
            CreateDateTime = createDateTime;
        }

        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsComplete { get; set; }
        public ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }

        public void AddElement(ProcessFlowInstanceElement elt)
        {
            Elements.Add(elt);
        }

        public ICollection<ProcessFlowInstanceElement> GetRunningElements()
        {
            return Elements.Where(e => e.Status == ProcessFlowInstanceElementStatus.Started).ToList();
        }

        public void AddConnector(string sourceNodeId, string targetNodeId)
        {
            Connectors.Add(new ProcessFlowConnector(Elements.First(n => n.Id == sourceNodeId), Elements.First(n => n.Id == targetNodeId)));
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

        public void Finish()
        {
            IsComplete = true;
        }

        public static ProcessFlowInstance New()
        {
            return new ProcessFlowInstance(Guid.NewGuid().ToString(), DateTime.UtcNow);
        }
    }
}