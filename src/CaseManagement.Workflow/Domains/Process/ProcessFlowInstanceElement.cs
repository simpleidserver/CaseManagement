using CaseManagement.Workflow.Domains.Process.Exceptions;
using System;
using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains
{
    public abstract class ProcessFlowInstanceElement : ICloneable
    {
        public ProcessFlowInstanceElement(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public abstract string ElementType { get; }
        public ProcessFlowInstanceElementStatus? Status { get; set; }
        public FormInstanceAggregate FormInstance { get; set; }

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

        public void SetFormInstance(FormInstanceAggregate formInstance)
        {
            FormInstance = formInstance;
        }

        public abstract void HandleLaunch();

        public abstract void HandleEvent(string state);

        public abstract object Clone();
    }
}
