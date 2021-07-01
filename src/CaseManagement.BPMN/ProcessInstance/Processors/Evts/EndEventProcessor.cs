using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Evts
{
    public class EndEventProcessor : BaseCatchEventProcessor<EndEvent>
    {
        protected override Task InternalHandle(BPMNExecutionContext executionContext, EndEvent nodeDef, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}