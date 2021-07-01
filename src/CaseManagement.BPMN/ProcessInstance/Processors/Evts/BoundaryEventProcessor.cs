using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Evts
{
    public class BoundaryEventProcessor : BaseCatchEventProcessor<BoundaryEvent>
    {
        protected override Task InternalHandle(BPMNExecutionContext executionContext, BoundaryEvent nodeDef, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
