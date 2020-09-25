using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Resources;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class InclusiveGatewayProcessor : BaseFlowNodeProcessor<InclusiveGateway>
    {
        protected override Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, InclusiveGateway elt, CancellationToken cancellationToken)
        {
            var flowNodeIds = GetNextFlowNodeIds(executionContext, elt);
            if (flowNodeIds.Count() == 0 && string.IsNullOrWhiteSpace(elt.Default))
            {
                throw new BPMNProcessorException(Global.NoDefaultSequenceFlow);
            }

            if (flowNodeIds.Count() == 0)
            {
                flowNodeIds.Add(elt.Default);
            }

            return Task.FromResult(BPMNExecutionResult.Next(flowNodeIds, executionContext.Pointer.Incoming));
        }
    }
}
