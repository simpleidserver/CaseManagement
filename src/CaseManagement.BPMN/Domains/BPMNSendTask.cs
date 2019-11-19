using System.Collections.Generic;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNSendTask : BPMNProcessFlowElement
    {
        public BPMNSendTask(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public static BPMNSendTask Build(ICollection<tFlowElement> flowElements, tSendTask sendTask)
        {
            return new BPMNSendTask(sendTask.name);
        }
    }
}
