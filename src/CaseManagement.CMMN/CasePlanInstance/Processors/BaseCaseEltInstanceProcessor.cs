using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseCaseEltInstanceProcessor<T> : IProcessor<T> where T : BaseCaseEltInstance
    {
        public BaseCaseEltInstanceProcessor(ISubscriberRepository subscriberRepository)
        {
            SubscriberRepository = subscriberRepository;
        }

        public Task Execute(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken)
        {
            if (elt.LatestTransition == null)
            {
                executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Create);
            }

            return Handle(executionContext, elt, cancellationToken);
        }

        protected ISubscriberRepository SubscriberRepository { get; private set; }

        protected Task<Subscription> TrySubscribe(CMMNExecutionContext executionContext, T casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TrySubscribe(executionContext.CasePlanInstance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected Task<bool> TryReset(CMMNExecutionContext executionContext, T casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TryReset(executionContext.CasePlanInstance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected abstract Task Handle(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
