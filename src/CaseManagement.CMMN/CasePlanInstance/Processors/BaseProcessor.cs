using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseProcessor<T> : IProcessor<T> where T : CasePlanElementInstance
    {
        public BaseProcessor(ISubscriberRepository subscriberRepository)
        {
            SubscriberRepository = subscriberRepository;
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

        public abstract Task Execute(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}