using CaseManagement.BPMN.Domains;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Factories
{
    public static class BPMNProcessFlowElementFactory
    {
        public static BPMNProcessFlowElement Build(ICollection<tFlowElement> flowElements, tFlowElement flowElement)
        {
            if (flowElement is tSendTask)
            {
                return BPMNSendTask.Build(flowElements, (tSendTask)flowElement);
            }

            if (flowElement is tExclusiveGateway)
            {
                return BPMNExecutiveGateway.Build(flowElements, (tExclusiveGateway)flowElement);
            }

            return null;
        }
    }
}
