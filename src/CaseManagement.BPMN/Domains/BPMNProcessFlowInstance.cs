using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNProcessFlowInstance
    {
        public BPMNProcessFlowInstance()
        {
        }

        public string ProcessId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public BPMNProcessFlowElement StartElement { get; set; }

        public static BPMNProcessFlowInstance Build(ICollection<tFlowElement> flowElements, string receiveTaskId)
        {
            var startElement = (tReceiveTask)flowElements.FirstOrDefault(i => i is tReceiveTask && i.id == receiveTaskId);
            if (startElement == null)
            {
                return null;
            }

            return new BPMNProcessFlowInstance
            {
                CreateDateTime = DateTime.UtcNow,
                StartElement = BPMNReceiveTask.Build(flowElements, startElement)
            };
        }
    }
}