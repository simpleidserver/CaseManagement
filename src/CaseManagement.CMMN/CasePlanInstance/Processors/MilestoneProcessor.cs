using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class MilestoneProcessor : BaseMilestoneOrTimerProcessor<MilestoneElementInstance>
    {
        public MilestoneProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override Task ProtectedProcess(CMMNExecutionContext executionContext, MilestoneElementInstance elt, CancellationToken cancellationToken)
        {
            executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Occur);
            return Task.CompletedTask;
        }
    }
}
