using System;

namespace CaseManagement.BPMN.Domains
{
    public class SequenceFlow : BaseFlowElement, ICloneable
    {
        public string SourceRef { get; set; }
        public string TargetRef { get; set; }
        public string ConditionExpression { get; set; }

        public object Clone()
        {
            return new SequenceFlow
            {
                SourceRef = SourceRef,
                TargetRef = TargetRef,
                ConditionExpression = ConditionExpression
            };
        }
    }
}
