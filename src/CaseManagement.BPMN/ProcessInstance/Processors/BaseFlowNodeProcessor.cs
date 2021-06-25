using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public abstract class BaseFlowNodeProcessor<TElt> : IProcessor<BPMNExecutionContext, TElt, ProcessInstanceAggregate> where TElt : BaseFlowNode
    {
        public async Task<ExecutionResult> Execute(BPMNExecutionContext executionContext, TElt elt, CancellationToken cancellationToken)
        {
            return await Handle(executionContext, elt, cancellationToken);
        }

        protected abstract Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, TElt elt, CancellationToken cancellationToken);
       
        protected virtual ICollection<string> GetNextFlowNodeIds(BPMNExecutionContext executionContext, BaseFlowNode flowNode)
        {
            var outgoing = executionContext.Instance.GetOutgoingSequenceFlows(flowNode.EltId);
            var filteredOutgoing = outgoing.Where(_ => executionContext.Instance.IsIncomingSatisfied(_, executionContext.Pointer.Incoming));
            return filteredOutgoing.Select(_ => _.TargetRef).ToList();
        }

        protected virtual ICollection<string> GetOutgoingFlowNodeIds(BPMNExecutionContext executionContext, BaseFlowNode flowNode)
        {
            return executionContext.Instance.GetOutgoingSequenceFlows(flowNode.EltId).Select(_ => _.TargetRef).ToList();
        }

        protected virtual ICollection<string> GetIncomingFlowNodeIds(BPMNExecutionContext executionContext, BaseFlowNode flowNode)
        {
            return executionContext.Instance.GetIncomingSequenceFlows(flowNode.EltId).Select(_ => _.SourceRef).ToList();
        }
    }
}