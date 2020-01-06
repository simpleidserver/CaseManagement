using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class CloseCommandHandler : ICloseCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IQueueProvider _queueProvider;

        public CloseCommandHandler(IEventStoreRepository eventStoreRepository, IQueueProvider queueProvider)
        {
            _eventStoreRepository = eventStoreRepository;
            _queueProvider = queueProvider;
        }

        public async Task Handle(CloseCommand closeCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CMMNWorkflowInstance>(closeCommand.CaseInstanceId, CMMNWorkflowInstance.GetStreamName(closeCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(closeCommand.CaseInstanceId);
            }

            caseInstance.MakeTransition(CMMNTransitions.Close);
            await _queueProvider.QueueTransition(caseInstance.Id, null, CMMNTransitions.Close);
        }
    }
}
