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
            if (!executionContext.Instance.IsEntryCriteriaSatisfied(elt))
            {
                return;
            }

            var createNewOccurrence = await Process(executionContext, elt, cancellationToken);
            if (createNewOccurrence && executionContext.Instance.IsRepetitionRuleSatisfied(elt))
            {
                var result = executionContext.Instance.TryCreateInstance(elt) as T;
                await Handle(executionContext, result, cancellationToken);
            }
        }

        protected abstract Task<bool> Process(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}