using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class StopCaseInstanceCommandHandler : IStopCaseInstanceCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public StopCaseInstanceCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task<bool> Handle(StopCaseInstanceCommand command)
        {
            /*
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CMMNProcessFlowInstance>(command.CaseInstanceId, CMMNProcessFlowInstance.GetCMMNStreamName(command.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(command.CaseInstanceId);
            }

            await _queueProvider.QueueStopProcess(command.CaseInstanceId);
            return true;
            */
            return true;
        }
    }
}
