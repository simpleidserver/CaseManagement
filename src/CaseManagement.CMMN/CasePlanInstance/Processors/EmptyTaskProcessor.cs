using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CasePlanInstance;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class EmptyTaskProcessor : BaseTaskOrStageProcessor<EmptyTaskElementInstance>
    {
        public EmptyTaskProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override Task ProtectedExecute(ExecutionContext executionContext, EmptyTaskElementInstance elt, CancellationToken cancellationToken)
        {
            executionContext.CasePlanInstance.MakeTransition(elt, Domains.CMMNTransitions.Complete);
            return Task.CompletedTask;
        }
    }
}