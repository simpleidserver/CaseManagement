using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstance
    {
        public ProcessFlowInstance()
        {
            Elements = new List<ProcessFlowInstanceElement>();
            Connectors = new List<ProcessFlowConnector>();
        }

        public ProcessFlowInstance(ProcessFlowInstanceElement startElement) : this()
        {
            StartElement = startElement;
            AddElement(startElement);
        }

        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsComplete { get; set; }
        public ProcessFlowInstanceElement StartElement { get; set; }
        public ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }

        public void AddElement(ProcessFlowInstanceElement elt)
        {
            Elements.Add(elt);
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

        public void Finish()
        {
            IsComplete = true;
        }
    }
}