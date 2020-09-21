using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseCasePlanItemProcessor<T> : BaseCaseEltInstanceProcessor<T> where T : BaseCasePlanItemInstance
    {
        protected BaseCasePlanItemProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override async Task Handle(ExecutionContext<CasePlanInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken)
        {
            var isNewElt = elt.LatestTransition == CMMNTransitions.Create;
            if (!executionContext.Instance.IsEntryCriteriaSatisfied(elt))
            {
                return;
            }

            await Process(executionContext, elt, cancellationToken);
            if (isNewElt && executionContext.Instance.IsRepetitionRuleSatisfied(elt))
            {
                executionContext.Instance.TryCreateInstance(elt);
            }
        }

        protected abstract Task Process(ExecutionContext<CasePlanInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken);
    }
}