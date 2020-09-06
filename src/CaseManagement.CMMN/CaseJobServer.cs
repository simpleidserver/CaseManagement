using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN
{
    public class CaseJobServer : ICaseJobServer
    {
        private readonly IEnumerable<IJob> _jobs;
        private readonly ICasePlanInstanceCommandRepository _casePlanInstanceCommandRepository;
        private readonly IMessageBroker _messageBroker;

        public CaseJobServer(IEnumerable<IJob> jobs, ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository, IMessageBroker messageBroker)
        {
            _jobs = jobs;
            _casePlanInstanceCommandRepository = casePlanInstanceCommandRepository;
            _messageBroker = messageBroker;
        }

        public void Start()
        {
            foreach(var job in _jobs)
            {
                job.Start();
            }
        }

        public void Stop()
        {
            foreach(var job in _jobs)
            {
                job.Stop();
            }
        }

        public Task PublishExternalEvt(string evt, string casePlanInstanceId, string casePlanElementInstanceId)
        {
            return _messageBroker.QueueExternalEvent(evt, casePlanInstanceId, casePlanElementInstanceId);
        }

        public async Task RegisterCasePlanInstance(CasePlanInstanceAggregate casePlanInstance, CancellationToken cancellationToken)
        {
            _casePlanInstanceCommandRepository.Add(casePlanInstance);
            await _casePlanInstanceCommandRepository.SaveChanges(cancellationToken);
        }

        public Task EnqueueCasePlanInstance(string id)
        {
            return _messageBroker.QueueCasePlanInstance(id);
        }
    }
}
