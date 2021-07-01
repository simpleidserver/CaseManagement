using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Evts
{
    public class StartEventProcessor : BaseCatchEventProcessor<StartEvent>
    {
        protected override Task InternalHandle(BPMNExecutionContext executionContext, StartEvent nodeDef, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}