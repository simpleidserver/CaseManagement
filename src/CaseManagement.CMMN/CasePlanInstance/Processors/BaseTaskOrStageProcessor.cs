using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseTaskOrStageProcessor : BaseCasePlanItemProcessor
    {
        public BaseTaskOrStageProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task<bool> Process(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken)
        {
            Subscription sub = null;
            var terminate = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, cancellationToken);
            var manualStart = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.ManualStart, cancellationToken);
            var disable = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Disable, cancellationToken);
            var reenable = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Reenable, cancellationToken);
            if (elt.TakeStageState == null)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Create);
            }

            if (elt.TakeStageState == TaskStageStates.Available)
            {
                if (elt.ManualActivationRule != null && elt.IsManualActivationRuleSatisfied(executionContext.Instance.ExecutionContext))
                {
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Enable);
                    return false;
                }

                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Start);
            }

            if (elt.TakeStageState == TaskStageStates.Enabled)
            {
                if (disable.IsCaptured)
                {
                    sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Disable, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Disable, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                    return false;
                }

                if (!manualStart.IsCaptured)
                {
                    return false;
                }

                sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.ManualStart, cancellationToken);
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.ManualStart, incomingTokens: MergeParameters(executionContext, sub.Parameters));
            }

            if (elt.TakeStageState == TaskStageStates.Disabled) 
            {
                if (reenable.IsCaptured)
                {
                    sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Reenable, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Reenable, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                }

                return false;
            }

            if (elt.TakeStageState == TaskStageStates.Active)
            {
                try
                {
                    if (terminate.IsCaptured)
                    {
                        sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, cancellationToken);
                        executionContext.Instance.MakeTransition(elt, CMMNTransitions.Terminate, incomingTokens: MergeParameters(executionContext, sub.Parameters));
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

        protected abstract Task<bool> ProtectedProcess(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken);
    }
}
