using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class EmptyTaskProcessor : BaseTaskOrStageProcessor<EmptyTaskElementInstance>
    {
        public EmptyTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override Task ProtectedProcess(CMMNExecutionContext executionContext, EmptyTaskElementInstance elt, CancellationToken cancellationToken)
        {
            executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Complete);
            return Task.CompletedTask;
        }
    }
}
