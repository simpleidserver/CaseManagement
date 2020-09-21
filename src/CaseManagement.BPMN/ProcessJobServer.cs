using CaseManagement.BPMN.Domains;
using CaseManagement.Common;
using CaseManagement.Common.Bus;
using CaseManagement.Common.Jobs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN
{
    public class ProcessJobServer : IProcessJobServer
    {
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEnumerable<IJob> _jobs;
        private readonly IMessageBroker _messageBroker;

        public ProcessJobServer(ICommitAggregateHelper commitAggregateHelper, IEnumerable<IJob> jobs, IMessageBroker messageBroker)
        {
            _commitAggregateHelper = commitAggregateHelper;
            _jobs = jobs;
            _messageBroker = messageBroker;
        }

        public Task EnqueueProcessInstance(string processInstanceId, CancellationToken token)
        {
            return _messageBroker.QueueProcessInstance(processInstanceId, token);
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

        public async Task RegisterProcessInstance(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            await _commitAggregateHelper.Commit(processInstance, processInstance.GetStreamName(), token);
        }
    }
}
