using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseCasePlanItemProcessor : BaseCaseEltInstanceProcessor
    {
        protected BaseCasePlanItemProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository)
        {
        }

        protected override async Task Handle(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken)
        {
            var entryCriteria = executionContext.Instance.IsEntryCriteriaSatisfied(elt);
            if (!entryCriteria.IsSatisfied)
            {
                return;
            }

            bool firstInstance = false;
            if (elt.LatestTransition == null)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Create);
                firstInstance = true;
            }

            var newExecutionContext = executionContext.NewExecutionContext(entryCriteria.Data);
            if (newExecutionContext.Instance.IsRepetitionRuleSatisfied(elt) && firstInstance && !entryCriteria.IsEmpty)
            {
                var result = newExecutionContext.Instance.TryCreateInstance(elt);
                await Handle(newExecutionContext, result, cancellationToken);
            }

            var mustCreate = await Process(executionContext, elt, cancellationToken);
            if (mustCreate && entryCriteria.IsEmpty && newExecutionContext.Instance.IsRepetitionRuleSatisfied(elt))
            {
                var result = newExecutionContext.Instance.TryCreateInstance(elt);
                await Handle(newExecutionContext, result, cancellationToken);
            }
        }

        protected abstract Task<bool> Process(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken);
    }
}