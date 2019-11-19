using CaseManagement.BPMN.Factories;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNSequenceFlow : BPMNProcessFlowElement
    {
        public BPMNSequenceFlow(BPMNProcessFlowElement target)
        {
            Target = target;
        }

        public BPMNProcessFlowElement Target { get; set; }

        public static BPMNSequenceFlow Build(ICollection<tFlowElement> flowElements, tSequenceFlow sequenceFlow)
        {
            var target = flowElements.First(f => f.id == sequenceFlow.targetRef);
            return new BPMNSequenceFlow(BPMNProcessFlowElementFactory.Build(flowElements, target));
        }
    }
}
