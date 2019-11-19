using System.Collections.Generic;

namespace CaseManagement.BPMN.Parser
{
    public class BPMNParsed
    {
        public BPMNParsed(ICollection<tProcess> processes)
        {
            Processes = processes;
        }

        public ICollection<tProcess> Processes { get; private set; }
    }
}
