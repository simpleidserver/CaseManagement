using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseActivationAggregate : ICloneable
    {
        public string WorkflowId { get; set; }
        public string WorkflowInstanceId { get; set; }
        public string WorkflowInstanceName { get; set; }
        public string WorkflowElementId { get; set; }
        public string WorkflowElementName { get; set; }
        public string WorkflowElementInstanceId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Performer { get; set; }
        
        public object Clone()
        {
            return new CaseActivationAggregate
            {
                WorkflowId = WorkflowId,
                WorkflowInstanceId = WorkflowInstanceId,
                WorkflowInstanceName = WorkflowInstanceName,
                WorkflowElementId = WorkflowElementId,
                WorkflowElementName = WorkflowElementName,
                WorkflowElementInstanceId = WorkflowElementInstanceId,
                CreateDateTime = CreateDateTime,
                Performer = Performer
            };
        }

        public override bool Equals(object obj)
        {
            var target = obj as CaseActivationAggregate;
            if (target == null)
            {
                return false;
            }

            return this.GetHashCode() == target.GetHashCode();
        }

        public override int GetHashCode()
        {
            return WorkflowElementInstanceId.GetHashCode();
        }
    }
}
