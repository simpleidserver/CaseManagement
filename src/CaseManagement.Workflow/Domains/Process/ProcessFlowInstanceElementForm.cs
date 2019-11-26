using CaseManagement.Workflow.Domains.Process;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceElementForm : ICloneable
    {
        private ProcessFlowInstanceElementForm(string id, string formId)
        {
            Id = id;
            FormId = formId;
            Content = new List<ProcessFlowInstanceElementFormElement>();
            Status = ProcessFlowInstanceElementFormStatus.Create;
        }

        public string Id { get; set; }
        public string FormId { get; set; }
        public ProcessFlowInstanceElementFormStatus Status { get; set; }
        public ICollection<ProcessFlowInstanceElementFormElement> Content { get; set; }

        public static ProcessFlowInstanceElementForm New(string formId)
        {
            return new ProcessFlowInstanceElementForm(Guid.NewGuid().ToString(), formId);
        }

        public object Clone()
        {
            return new ProcessFlowInstanceElementForm(Id, FormId)
            {
                Status = Status,
                Content = Content.Select(c => (ProcessFlowInstanceElementFormElement)c.Clone()).ToList()
            };
        }
    }
}
