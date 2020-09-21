using CaseManagement.CMMN.Domains;
using CaseManagement.Common;
using CaseManagement.Common.Bus;
using CaseManagement.Common.Jobs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN
{
    public class CaseJobServer : ICaseJobServer
    {
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEnumerable<IJob> _jobs;
        private readonly IMessageBroker _messageBroker;

        public CaseJobServer(ICommitAggregateHelper commitAggregateHelper, IEnumerable<IJob> jobs, IMessageBroker messageBroker)
        {
            _commitAggregateHelper = commitAggregateHelper;
            _jobs = jobs;
            _messageBroker = messageBroker;
        }

        public Task EnqueueCasePlanInstance(string casePlanInstanceId, CancellationToken token)
        {
            return _messageBroker.QueueCasePlanInstance(casePlanInstanceId, token);
        }

        public Task PublishExternalEvt(string evt, string casePlanInstanceId, string casePlanElementInstanceId, CancellationToken token)
        {
            return _messageBroker.QueueExternalEvent(evt, casePlanInstanceId, casePlanElementInstanceId, token);
        }

        public async Task RegisterCasePlanInstance(CasePlanInstanceAggregate casePlanInstance, CancellationToken token)
        {
            await _commitAggregateHelper.Commit(casePlanInstance, casePlanInstance.GetStreamName(), token);
        }

        public void Start()
        {
            foreach (var messageConsumer in _jobs)
            {
                messageConsumer.Start();
            }
        }

        public void Stop()
        {
            foreach (var messageConsumer in _jobs)
            {
                try
                {
                    messageConsumer.Stop().Wait();
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
