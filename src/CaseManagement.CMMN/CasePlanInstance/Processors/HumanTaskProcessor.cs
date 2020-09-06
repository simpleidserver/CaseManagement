using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CasePlanInstance;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Steps
{
    public class HumanTaskProcessor : BaseTaskOrStageProcessor<HumanTaskElementInstance>
    {
        public HumanTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override async Task ProtectedExecute(ExecutionContext executionContext, HumanTaskElementInstance elt, CancellationToken cancellationToken)
        {
            const string evtName = "complete";
            var subscription = await GetSubscription(executionContext, elt, evtName, cancellationToken);
            if (subscription == null)
            {
                await Subscribe(executionContext, elt, evtName, cancellationToken);
                return;
            }

            if (subscription.IsCaptured)
            {
                executionContext.CasePlanInstance.MakeTransition(elt, Domains.CMMNTransitions.Complete);
            }
        }
    }
}
