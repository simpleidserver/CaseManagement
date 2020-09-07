using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseProcessor<T> where T : CasePlanElementInstance
    {
        public BaseProcessor(ISubscriberRepository subscriberRepository)
        {
            SubscriberRepository = subscriberRepository;
        }

        protected ISubscriberRepository SubscriberRepository { get; private set; }

        protected Task<Subscription> TrySubscribe(ExecutionContext executionContext, T casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TrySubscribe(executionContext.CasePlanInstance.Id, casePlanElementInstance.Id, evtName, cancellationToken);
        } 

        public abstract Task Execute(ExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
