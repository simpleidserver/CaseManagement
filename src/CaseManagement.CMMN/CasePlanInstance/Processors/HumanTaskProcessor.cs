using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class HumanTaskProcessor : BaseTaskOrStageProcessor<HumanTaskElementInstance>
    {
        public HumanTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task ProtectedProcess(CMMNExecutionContext executionContext, HumanTaskElementInstance elt, CancellationToken cancellationToken)
        {
            executionContext.CasePlanInstance.TryAddWorkerTask(elt.Id);
            var completeSubscription = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Complete, cancellationToken);
            if (completeSubscription.IsCaptured)
            {
                executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Complete);
            }
        }
    }
}
