using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseTaskOrStageProcessor<T> : BaseCasePlanItemProcessor<T> where T : BaseTaskOrStageElementInstance
    {
        public BaseTaskOrStageProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task Process(ExecutionContext<CasePlanInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken)
        {
            var terminate = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, cancellationToken);
            var manualStart = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.ManualStart, cancellationToken);
            if (elt.State == null)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Create);
            }

            if (elt.State == TaskStageStates.Available)
            {
                if (elt.ManualActivationRule != null && elt.IsManualActivationRuleSatisfied(executionContext.Instance.ExecutionContext))
                {
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Enable);
                    return;
                }

                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Start);
            }

            if (elt.State == TaskStageStates.Enabled)
            {
                if (!manualStart.IsCaptured)
                {
                    return;
                }

                executionContext.Instance.MakeTransition(elt, CMMNTransitions.ManualStart);
            }

            if (elt.State == TaskStageStates.Active)
            {
                await ProtectedProcess(executionContext, elt, cancellationToken);
                if (terminate.IsCaptured)
                {
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Terminate);
                    return;
                }
            }
        }

        protected abstract Task ProtectedProcess(ExecutionContext<CasePlanInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken);
    }
}
