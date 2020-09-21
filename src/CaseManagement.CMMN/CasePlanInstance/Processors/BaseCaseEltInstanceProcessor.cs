using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseCaseEltInstanceProcessor<T> : IProcessor<CasePlanInstanceAggregate, T> where T : BaseCaseEltInstance
    {
        public BaseCaseEltInstanceProcessor(ISubscriberRepository subscriberRepository)
        {
            SubscriberRepository = subscriberRepository;
        }

        public Task Execute(ExecutionContext<CasePlanInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken)
        {
            if (elt.LatestTransition == null)
            {
                executionContext.Instance.MakeTransition(elt, CMMNTransitions.Create);
            }

            return Handle(executionContext, elt, cancellationToken);
        }

        protected ISubscriberRepository SubscriberRepository { get; private set; }

        protected Task<Subscription> TrySubscribe(ExecutionContext<CasePlanInstanceAggregate> executionContext, T casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TrySubscribe(executionContext.Instance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected Task<bool> TryReset(ExecutionContext<CasePlanInstanceAggregate> executionContext, T casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TryReset(executionContext.Instance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected abstract Task Handle(ExecutionContext<CasePlanInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken);

    }
}
