using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseMilestoneOrTimerProcessor : BaseCasePlanItemProcessor
    {
        public BaseMilestoneOrTimerProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task<bool> Process(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken token)
        {
            var terminateSubscription = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, token);
            if (elt.MilestoneState == MilestoneEventStates.Available)
            {
                await ProtectedProcess(executionContext, elt, token);
                if (terminateSubscription.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, token);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Terminate, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                }

                return true;
            }

            return false;
        }

        protected abstract Task ProtectedProcess(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken);
    }
}
