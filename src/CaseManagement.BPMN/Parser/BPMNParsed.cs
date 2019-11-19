using CaseManagement.BPMN.Domains;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Parser
{
    public class BPMNParsed
    {
        public BPMNParsed(tDefinitions definitions, ICollection<BPMNMessageElement> messageElements, ICollection<BPMNOperation> operations)
        {
            Definitions = definitions;
            MessageElements = messageElements;
            Operations = operations;
        }

        public tDefinitions Definitions { get; private set; }
        public ICollection<BPMNMessageElement> MessageElements { get; set; }
        public ICollection<BPMNOperation> Operations { get; set; }
    }
}
