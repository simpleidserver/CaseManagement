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

        protected Task<Subscription> GetSubscription(ExecutionContext executionContext, T casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.Get(executionContext.CasePlanInstance.Id, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected Task<bool> Subscribe(ExecutionContext executionContext, T casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.Add(new Subscription
            {
                CasePlanElementInstanceId = casePlanElementInstance.Id,
                CasePlanInstanceId = executionContext.CasePlanInstance.Id,
                EventName = evtName,
                IsCaptured = false
            }, cancellationToken);
        } 

        public abstract Task Execute(ExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
