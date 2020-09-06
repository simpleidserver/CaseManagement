using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.Workflow.Infrastructure.Bus;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class CloseCommandHandler : ICloseCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMessageBroker _messageBroker;

        public CloseCommandHandler(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _messageBroker = messageBroker;
        }

        public async Task Handle(CloseCommand closeCommand)
        {
            /*
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(closeCommand.CaseInstanceId, CasePlanInstanceAggregate.GetStreamName(closeCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(closeCommand.CaseInstanceId);
            }

            if (!closeCommand.BypassUserValidation && caseInstance.CaseOwner != closeCommand.Performer)
            {
                throw new UnauthorizedCaseWorkerException(closeCommand.Performer, closeCommand.CaseInstanceId, string.Empty);
            }

            if (caseInstance.State == Enum.GetName(typeof(CaseStates), CaseStates.Suspended))
            {
                caseInstance.MakeTransition(CMMNTransitions.Close);
                await _messageBroker.QueueTransition(caseInstance.Id, null, CMMNTransitions.Close);
            }
            else
            {
                caseInstance.MakeTransition(CMMNTransitions.Close);
                await _messageBroker.QueueEvent(caseInstance.DomainEvents.Last(), CMMNConstants.QueueNames.CasePlanInstances);
            }
            */
        }
    }
}
