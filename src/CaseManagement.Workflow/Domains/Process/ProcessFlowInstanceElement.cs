using CaseManagement.Workflow.Domains.Process.Exceptions;
using System;
using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceElement : ICloneable
    {
        public ProcessFlowInstanceElement(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ProcessFlowInstanceElementStatus? Status { get; set; }
        public ProcessFlowInstanceElementForm FormInstance { get; set; }

        public void Launch()
        {
            Status = ProcessFlowInstanceElementStatus.Launched;
            HandleLaunch();
        }

        public void Finish()
        {
            if (Status != ProcessFlowInstanceElementStatus.Finished)
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "validation_error", "process element is not started" }
                    }
                };
            }

            Status = ProcessFlowInstanceElementStatus.Finished;
        }

        public void SetFormInstance(ProcessFlowInstanceElementForm formInstance)
        {
            FormInstance = formInstance;
        }

        public virtual void HandleLaunch() { }

        public virtual void HandleEvent(string state) { }

        public virtual object Clone()
        {
            return new ProcessFlowInstanceElement(Id, Name)
            {
                Status = Status
            };
        }
    }
}
