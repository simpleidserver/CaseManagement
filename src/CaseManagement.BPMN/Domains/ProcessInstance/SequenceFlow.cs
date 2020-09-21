using System.Collections.Generic;

namespace CaseManagement.BPMN.Domains.ProcessInstance
{
    public class SequenceFlow : BaseFlowElement
    {
        public SequenceFlow()
        {
            Nodes = new List<BaseFlowNode>();
        }

        public ICollection<BaseFlowNode> Nodes { get; set; }
    }
}
