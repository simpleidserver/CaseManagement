using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public abstract class BaseActivityProcessor<T>: BaseFlowNodeProcessor<T> where T : BaseActivity
    {
        protected override async Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, T elt,  CancellationToken cancellationToken)
        {
            // Page : 428 : BPMN2.0.2
            var pointer = executionContext.Pointer;
            var instance = executionContext.Instance.GetInstance(pointer.InstanceFlowNodeId);
            if (instance.ActivityState == null)
            {
                executionContext.Instance.UpdateState(instance, ActivityStates.READY);
            }

            if (instance.ActivityState == ActivityStates.READY)
            {
                executionContext.Instance.UpdateState(instance, ActivityStates.ACTIVE);
            }

            if (instance.ActivityState == ActivityStates.ACTIVE)
            {
                executionContext.Instance.UpdateState(instance, ActivityStates.COMPLETING);
            }

            var outcome = new List<BaseToken>();
            outcome.AddRange(executionContext.Pointer.Incoming);
            if (instance.ActivityState == ActivityStates.COMPLETING)
            {
                var addOutcome = await Process(executionContext, elt, cancellationToken);
                outcome.AddRange(addOutcome);
                executionContext.Instance.UpdateState(instance, ActivityStates.COMPLETED);
            }

            return BPMNExecutionResult.Outcome(outcome);
        }

        protected abstract Task<ICollection<BaseToken>> Process(BPMNExecutionContext context, T elt,  CancellationToken cancellationToken);
    }
}
