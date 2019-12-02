using System;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceElement : ICloneable
    {
        public ProcessFlowInstanceElement(string id, string name)
        {
            Id = id;
            Name = name;
            Status = ProcessFlowInstanceElementStatus.None;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ProcessFlowInstanceElementStatus Status { get; private set; }
        public ProcessFlowInstanceElementForm FormInstance { get; private set; }

        internal void Run()
        {
            Status = ProcessFlowInstanceElementStatus.Started;
        }

        internal void Finish()
        {
            Status = ProcessFlowInstanceElementStatus.Finished;
        }

        public void SetFormInstance(ProcessFlowInstanceElementForm formInstance)
        {
            FormInstance = formInstance;
        }

        public virtual object Clone()
        {
            return new ProcessFlowInstanceElement(Id, Name)
            {
                Status = Status,
                FormInstance = FormInstance == null ? null : (ProcessFlowInstanceElementForm)FormInstance.Clone()
            };
        }
    }
}
