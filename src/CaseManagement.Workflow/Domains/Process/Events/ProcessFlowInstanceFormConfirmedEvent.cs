using CaseManagement.Workflow.Infrastructure;
using Newtonsoft.Json.Linq;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceFormConfirmedEvent : DomainEvent
    {
        public ProcessFlowInstanceFormConfirmedEvent(string id, string aggregateId, int version, string elementId, Form form, JObject content) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            Form = form;
            Content = content;
        }

        public string ElementId { get; set; }
        public Form Form { get; set; }
        public JObject Content { get; set; }
    }
}
