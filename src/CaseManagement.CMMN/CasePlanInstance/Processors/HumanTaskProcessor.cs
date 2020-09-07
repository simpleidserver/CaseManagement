using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Steps
{
    public class HumanTaskProcessor : BaseTaskOrStageProcessor<HumanTaskElementInstance>
    {
        public HumanTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        protected override async Task ProtectedExecute(ExecutionContext executionContext, HumanTaskElementInstance elt, CancellationToken cancellationToken)
        {
            var subscription = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Complete, cancellationToken);
            if (subscription.IsCaptured)
            {
                executionContext.CasePlanInstance.MakeTransition(elt, Domains.CMMNTransitions.Complete);
            }
        }
    }
}
