using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNReceiveTask : BPMNProcessFlowElement
    {
        public BPMNReceiveTask(string name, bool instantiate)
        {
            Name = name;
            Instantiate = instantiate;
            SequenceFlows = new List<BPMNSequenceFlow>();
        }

        public string Name { get; set; }
        public bool Instantiate { get; set; }
        public ICollection<BPMNSequenceFlow> SequenceFlows { get; set; }

        public static BPMNReceiveTask Build(ICollection<tFlowElement> flowElements, tReceiveTask receiveTask)
        {
            var sequenceFlows = flowElements.Where(i => i is tSequenceFlow && ((tSequenceFlow)i).sourceRef == receiveTask.id).Cast<tSequenceFlow>();
            var result = new BPMNReceiveTask(receiveTask.name, receiveTask.instantiate);
            foreach(var sequenceFlow in sequenceFlows)
            {
                result.SequenceFlows.Add(BPMNSequenceFlow.Build(flowElements, sequenceFlow));
            }

            return result;
        }
    }
}
