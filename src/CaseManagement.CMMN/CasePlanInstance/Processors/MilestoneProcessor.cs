using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class MilestoneProcessor : BaseMilestoneOrTimerProcessor
    {
        public MilestoneProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        public override CasePlanElementInstanceTypes Type => CasePlanElementInstanceTypes.MILESTONE;

        protected override Task ProtectedProcess(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken)
        {
            executionContext.Instance.MakeTransition(elt, CMMNTransitions.Occur);
            return Task.CompletedTask;
        }
    }
}
