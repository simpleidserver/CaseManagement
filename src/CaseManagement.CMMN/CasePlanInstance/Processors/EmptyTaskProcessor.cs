using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class EmptyTaskProcessor : BaseTaskOrStageProcessor<EmptyTaskElementInstance>
    {
        public EmptyTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override Task<bool> ProtectedProcess(CMMNExecutionContext executionContext, EmptyTaskElementInstance elt, CancellationToken cancellationToken)
        {
            executionContext.Instance.MakeTransition(elt, CMMNTransitions.Complete);
            return Task.FromResult(true);
        }
    }
}
