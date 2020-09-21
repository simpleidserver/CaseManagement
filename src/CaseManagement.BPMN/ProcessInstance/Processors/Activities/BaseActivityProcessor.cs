using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public abstract class BaseActivityProcessor<T> : BaseFlowNodeProcessor<T> where T : BaseActivity
    {
        protected override async Task Handle(ExecutionContext<ProcessInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken)
        {
            // Page : 428 : BPMN2.0.2
            if (elt.State == null)
            {
                // TODO : Check number of tokens.
                executionContext.Instance.MakeTransition(elt, BPMNTransitions.ACTIVITYREADY);
            }

            if (elt.State == ActivityStates.READY)
            {
                // TODO : Check Data input Set.
                executionContext.Instance.MakeTransition(elt, BPMNTransitions.ACTIVITYACTIVE);
            }

            if (elt.State == ActivityStates.ACTIVE)
            {
                await Process(executionContext, elt, cancellationToken);
                executionContext.Instance.MakeTransition(elt, BPMNTransitions.COMPLETE);
            }
        }

        protected abstract Task Process(ExecutionContext<ProcessInstanceAggregate> context, T elt, CancellationToken cancellationToken);
    }
}
