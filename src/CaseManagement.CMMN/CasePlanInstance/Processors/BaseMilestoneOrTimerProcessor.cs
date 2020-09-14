using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseMilestoneOrTimerProcessor<T> : BaseCasePlanItemProcessor<T> where T : BaseMilestoneOrTimerElementInstance
    {
        public BaseMilestoneOrTimerProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task Process(CMMNExecutionContext executionContext, T elt, CancellationToken token)
        {
            var terminateSubscription = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, token);
            if (elt.State == MilestoneEventStates.Available)
            {
                await ProtectedProcess(executionContext, elt, token);
                if (terminateSubscription.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Terminate);
                }

                return;
            }
        }

        protected abstract Task ProtectedProcess(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
