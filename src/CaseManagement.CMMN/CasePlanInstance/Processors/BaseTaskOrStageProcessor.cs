using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseTaskOrStageProcessor<T> : BaseCasePlanItemProcessor<T> where T : BaseTaskOrStageElementInstance
    {
        public BaseTaskOrStageProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task<bool> Process(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken)
        {
            var terminate = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, cancellationToken);
            var manualStart = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.ManualStart, cancellationToken);
            var disable = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Disable, cancellationToken);
            if (elt.State == null)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Create);
            }

            if (elt.State == TaskStageStates.Available)
            {
                if (elt.ManualActivationRule != null && elt.IsManualActivationRuleSatisfied(executionContext.Instance.ExecutionContext))
                {
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Enable);
                    return false;
                }

                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Start);
            }

            if (elt.State == TaskStageStates.Enabled)
            {
                if (disable.IsCaptured)
                {
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Disable);
                    return false;
                }

                if (!manualStart.IsCaptured)
                {
                    return false;
                }

                executionContext.Instance.MakeTransition(elt, CMMNTransitions.ManualStart);
            }

            if (elt.State == TaskStageStates.Active)
            {
                try
                {
                    if (terminate.IsCaptured)
                    {
                        executionContext.Instance.MakeTransition(elt, CMMNTransitions.Terminate);
                        return true;
                    }

                    return await ProtectedProcess(executionContext, elt, cancellationToken);
                }
                catch(Exception ex)
                {
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Fault, ex.ToString());
                    return false;
                }
            }

            return false;
        }

        protected abstract Task<bool> ProtectedProcess(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
