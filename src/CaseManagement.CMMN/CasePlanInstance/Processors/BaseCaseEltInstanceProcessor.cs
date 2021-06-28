using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Processors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseCaseEltInstanceProcessor : ICaseEltInstanceProcessor
    {
        public BaseCaseEltInstanceProcessor(ISubscriberRepository subscriberRepository)
        {
            SubscriberRepository = subscriberRepository;
        }

        public async Task<ExecutionResult> Execute(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken)
        {
            await Handle(executionContext, elt, cancellationToken);
            return ExecutionResult.Next();
        }

        public abstract CasePlanElementInstanceTypes Type { get; }

        protected ISubscriberRepository SubscriberRepository { get; private set; }

        protected Task<Subscription> TrySubscribe(CMMNExecutionContext executionContext, CaseEltInstance casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TrySubscribe(executionContext.Instance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected Task<Subscription> TryReset(CMMNExecutionContext executionContext, CaseEltInstance casePlanElementInstance, string evtName, CancellationToken cancellationToken)
        {
            return SubscriberRepository.TryReset(executionContext.Instance.AggregateId, casePlanElementInstance.Id, evtName, cancellationToken);
        }

        protected Dictionary<string, string> MergeParameters(CMMNExecutionContext executionContext, Dictionary<string, string> parameters)
        {
            var result = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach(var kvp in parameters)
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }

            if (executionContext.IncomingTokens != null)
            {
                foreach(var kvp in executionContext.IncomingTokens)
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }

            return result;
        }

        protected abstract Task Handle(CMMNExecutionContext executionContext, CaseEltInstance elt, CancellationToken cancellationToken);

    }
}
