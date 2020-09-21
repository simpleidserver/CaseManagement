using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public abstract class BaseFlowNodeProcessor<T> : IProcessor<ProcessInstanceAggregate, T> where T : BaseFlowNode
    {
        public async Task Execute(ExecutionContext<ProcessInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken)
        {
            if (elt.LastTransition == null)
            {
                if (CheckIncoming(executionContext, elt))
                {
                    executionContext.Instance.MakeTransition(elt, BPMNTransitions.CREATE);
                }
            }

            if (elt.LastTransition == BPMNTransitions.CREATE)
            {
                await Handle(executionContext, elt, cancellationToken);
            }
        }

        protected abstract Task Handle(ExecutionContext<ProcessInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken);

        protected virtual bool CheckIncoming(ExecutionContext<ProcessInstanceAggregate> executionContext, T elt)
        {
            return executionContext.Instance.IsIncomingSatisfied(elt);
        }
    }
}