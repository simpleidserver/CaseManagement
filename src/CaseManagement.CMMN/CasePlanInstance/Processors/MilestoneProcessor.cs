using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class MilestoneProcessor : BaseMilestoneOrTimerProcessor<MilestoneElementInstance>
    {
        public MilestoneProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override Task ProtectedProcess(CMMNExecutionContext executionContext, MilestoneElementInstance elt, CancellationToken cancellationToken)
        {
            executionContext.Instance.MakeTransition(elt, CMMNTransitions.Occur);
            return Task.CompletedTask;
        }
    }
}
