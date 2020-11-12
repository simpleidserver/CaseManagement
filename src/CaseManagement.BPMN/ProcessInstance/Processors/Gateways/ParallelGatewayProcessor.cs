using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Gateways
{
    public class ParallelGatewayProcessor : BaseFlowNodeProcessor<ParallelGateway>
    {
        protected override Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, ParallelGateway elt, CancellationToken cancellationToken)
        {
            var outgoingFlowNodeIds = GetOutgoingFlowNodeIds(executionContext, elt);
            var incomingFlowNodeIds = GetIncomingFlowNodeIds(executionContext, elt);
            if (elt.GatewayDirection == GatewayDirections.CONVERGING && outgoingFlowNodeIds.Count() != 1)
            {
                throw new BPMNProcessorException("Must have no more than one outgoing sequence flow");
            }

            if (elt.GatewayDirection == GatewayDirections.DIVERGING && incomingFlowNodeIds.Count() != 1)
            {
                throw new BPMNProcessorException("Must have no more than one incoming sequence flow");
            }

            var incoming = executionContext.Pointer.Incoming;
            ICollection<BaseToken> outcome = new List<BaseToken>();
            if (elt.GatewayDirection == GatewayDirections.CONVERGING)
            {
                if (incoming.Count() != incomingFlowNodeIds.Count())
                {
                    return Task.FromResult(BPMNExecutionResult.Block());
                }

                foreach(var record in incoming)
                {
                    if (!outcome.Any(_ => _.Name == record.Name))
                    {
                        outcome.Add(record);
                    }
                }
            }
            else
            {
                outcome = incoming;
            }

            return Task.FromResult(BPMNExecutionResult.Next(outgoingFlowNodeIds, outcome));
        }
    }
}
