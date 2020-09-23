using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseCaseEltInstanceProcessor<TElt> : IProcessor<CMMNExecutionContext, TElt, CasePlanInstanceAggregate> where TElt : BaseCaseEltInstance
    {
        public BaseCaseEltInstanceProcessor(ISubscriberRepository subscriberRepository)
        {
            SubscriberRepository = subscriberRepository;
        }

        public async Task<ExecutionResult> Execute(CMMNExecutionContext executionContext, TElt elt, CancellationToken cancellationToken)
        {
            if (elt.LatestTransition == null)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Create);
            }

            await Handle(executionContext, elt, cancellationToken);
            return ExecutionResult.Next();
        }

        protected ISubscriberRepository SubscriberRepository { get; private set; }

        protected Task<Subscription> TrySubscribe(CMMNExecutionContext executionContext, TElt casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TrySubscribe(executionContext.Instance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected Task<bool> TryReset(CMMNExecutionContext executionContext, TElt casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TryReset(executionContext.Instance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected abstract Task Handle(CMMNExecutionContext executionContext, TElt elt, CancellationToken cancellationToken);

    }
}
