using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Parser;
using System.Linq;

namespace CaseManagement.BPMN
{
    public class BPMNEngine
    {
        public void StartProcessInstanceOnReceiveTask(string processId, string taskId, BPMNParsed parsed)
        {
            var process = (tProcess)parsed.Definitions.Items.FirstOrDefault(i => i is tProcess && i.id == processId);
            if (process == null)
            {
                // THROW EXCEPTION.
            }

            var instance = BPMNProcessFlowInstance.Build(process.Items, taskId);
        }
    }
}