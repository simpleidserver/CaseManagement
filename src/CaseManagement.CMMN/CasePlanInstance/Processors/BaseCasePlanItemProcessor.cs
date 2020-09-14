using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseCasePlanItemProcessor<T> : BaseCaseEltInstanceProcessor<T> where T : BaseCasePlanItemInstance
    {
        protected BaseCasePlanItemProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override async Task Handle(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken)
        {
            var isNewElt = elt.LatestTransition == CMMNTransitions.Create;
            if (!executionContext.CasePlanInstance.IsEntryCriteriaSatisfied(elt))
            {
                return;
            }

            await Process(executionContext, elt, cancellationToken);
            if (isNewElt && executionContext.CasePlanInstance.IsRepetitionRuleSatisfied(elt))
            {
                executionContext.CasePlanInstance.TryCreateInstance(elt);
            }
        }

        protected abstract Task Process(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}