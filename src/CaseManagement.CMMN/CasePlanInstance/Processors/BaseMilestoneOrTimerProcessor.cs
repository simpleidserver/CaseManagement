using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseMilestoneOrTimerProcessor<T> : BaseProcessor<T> where T : BaseMilestoneOrTimerElementInstance
    {
        public BaseMilestoneOrTimerProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        public override async Task Execute(CMMNExecutionContext executionContext, T elt, CancellationToken token)
        {
            var terminateSubscription = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Terminate, token);
            if (elt.State == null)
            {
                executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Create);
            }

            if (elt.State == MilestoneEventStates.Available)
            {
                await ProtectedExecute(executionContext, elt, token);
                if (terminateSubscription.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Terminate);
                    return;
                }
            }
        }

        protected abstract Task ProtectedExecute(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
