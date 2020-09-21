using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseWorkerTask
{
    public class CaseWorkerTaskHandler : 
        IDomainEvtConsumerGeneric<CaseInstanceWorkerTaskAddedEvent>,
        IDomainEvtConsumerGeneric<CaseInstanceWorkerTaskRemovedEvent>
    {
        private readonly ICaseWorkerTaskCommandRepository _caseWorkerTaskCommandRepository;
        private readonly ICaseWorkerTaskQueryRepository _caseWorkerTaskQueryRepository;

        public CaseWorkerTaskHandler(ICaseWorkerTaskCommandRepository caseWorkerTaskCommandRepository, ICaseWorkerTaskQueryRepository caseWorkerTaskQueryRepository)
        {
            _caseWorkerTaskCommandRepository = caseWorkerTaskCommandRepository;
            _caseWorkerTaskQueryRepository = caseWorkerTaskQueryRepository;
        }

        public async Task Handle(CaseInstanceWorkerTaskAddedEvent message, CancellationToken token)
        {
            var caseWorkerTask = CaseWorkerTaskAggregate.New(message);
            await _caseWorkerTaskCommandRepository.Add(caseWorkerTask, token);
            await _caseWorkerTaskCommandRepository.SaveChanges(token);
        }

        public async Task Handle(CaseInstanceWorkerTaskRemovedEvent message, CancellationToken token)
        {
            var id = CaseWorkerTaskAggregate.BuildCaseWorkerTaskIdentifier(message.AggregateId, message.CasePlanInstanceElementId);
            var result = await _caseWorkerTaskQueryRepository.Get(id, token);
            await _caseWorkerTaskCommandRepository.Delete(result, token);
            await _caseWorkerTaskCommandRepository.SaveChanges(token);
        }
    }
}
