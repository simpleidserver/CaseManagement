using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class EmptyTaskProcessor : BaseTaskOrStageProcessor
    {
        public EmptyTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        public override CasePlanElementInstanceTypes Type => CasePlanElementInstanceTypes.EMPTYTASK;

        protected override Task<bool> ProtectedProcess(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken)
        {
            executionContext.Instance.MakeTransition(elt, CMMNTransitions.Complete);
            return Task.FromResult(true);
        }
    }
}
