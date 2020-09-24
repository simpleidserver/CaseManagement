using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class ExclusiveGatewayProcessor : BaseFlowNodeProcessor<ExclusiveGateway>
    {
        protected override Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, ExclusiveGateway elt, CancellationToken cancellationToken)
        {
            var flowNodeIds = GetNextFlowNodeIds(executionContext, elt);
            if (flowNodeIds.Count() > 1)
            {
                throw new BPMNProcessorException("There is more than one 'conditionalExpression' satisfied");
            }

            if (flowNodeIds.Count() == 0 && string.IsNullOrWhiteSpace(elt.Default))
            {
                throw new BPMNProcessorException("There is no default 'sequenceFlow'");
            }

            if (flowNodeIds.Count() == 0)
            {
                flowNodeIds.Add(elt.Default);
            }

            return Task.FromResult(BPMNExecutionResult.Next(flowNodeIds, executionContext.Pointer.Incoming));
        }
    }
}
