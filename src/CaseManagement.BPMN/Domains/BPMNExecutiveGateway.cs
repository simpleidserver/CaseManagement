using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNExecutiveGateway : BPMNProcessFlowElement
    {
        public BPMNExecutiveGateway(BPMNSequenceFlow defaultFlow)
        {
            DefaultFlow = defaultFlow;
            Targets = new List<BPMNSequenceFlow>();
        }

        public BPMNExecutiveGateway(BPMNSequenceFlow defaultFlow, ICollection<BPMNSequenceFlow> targets) : this(defaultFlow)
        {
            Targets = targets;
        }

        public ICollection<BPMNSequenceFlow> Targets { get; set; }
        public BPMNSequenceFlow DefaultFlow { get; set; }

        public static BPMNExecutiveGateway Build(ICollection<tFlowElement> flowElements, tExclusiveGateway exclusiveGateway)
        {
            var sequenceFlows = flowElements.Where(f => f is tSequenceFlow && ((tSequenceFlow)f).sourceRef == exclusiveGateway.id).Cast<tSequenceFlow>();
            var defaultFlow = sequenceFlows.First(f => f.id == exclusiveGateway.@default);
            var result = new BPMNExecutiveGateway(BPMNSequenceFlow.Build(flowElements, defaultFlow));
            foreach(var sequenceFlow in sequenceFlows.Where(f => f.id != exclusiveGateway.@default))
            {
                result.Targets.Add(BPMNSequenceFlow.Build(flowElements, sequenceFlow));
            }

            return result;
        }
    }
}
