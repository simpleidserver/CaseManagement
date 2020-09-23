using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public abstract class BaseFlowNodeProcessor<TElt> : IProcessor<BPMNExecutionContext, TElt, ProcessInstanceAggregate> where TElt : BaseFlowNode
    {
        public async Task<ExecutionResult> Execute(BPMNExecutionContext executionContext, TElt elt, CancellationToken cancellationToken)
        {
            if (!CheckIncoming(executionContext, executionContext.Pointer))
            {
                return BPMNExecutionResult.Block();
            }

            return await Handle(executionContext, elt, cancellationToken);
        }

        protected abstract Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, TElt elt, CancellationToken cancellationToken);

        protected virtual bool CheckIncoming(BPMNExecutionContext executionContext, ExecutionPointer elt)
        {
            return executionContext.Instance.IsIncomingSatisfied(elt);
        }
    }
}