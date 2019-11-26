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

        public void Run()
        {
            Status = ProcessFlowInstanceElementStatus.Started;
        }

        public void Finish()
        {
            Status = ProcessFlowInstanceElementStatus.Finished;
        }

        public void SetFormInstance(Form form)
        {
            FormInstance = ProcessFlowInstanceElementForm.New(form.Id);
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
