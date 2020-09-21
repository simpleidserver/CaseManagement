using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class HumanTaskProcessor : BaseTaskOrStageProcessor<HumanTaskElementInstance>
    {
        public HumanTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task ProtectedProcess(ExecutionContext<CasePlanInstanceAggregate> executionContext, HumanTaskElementInstance elt, CancellationToken cancellationToken)
        {
            executionContext.Instance.TryAddWorkerTask(elt.Id);
            var completeSubscription = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Complete, cancellationToken);
            if (completeSubscription.IsCaptured)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Complete);
            }
        }
    }
}
