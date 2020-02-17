using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class ActivateCommandHandler : IActivateCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ActivateCommandHandler(IEventStoreRepository eventStoreRepository, IRoleQueryRepository roleQueryRepository, IMessageBroker messageBroker, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _roleQueryRepository = roleQueryRepository;
            _messageBroker = messageBroker;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task Handle(ActivateCommand activateCommand)
        {
            var id = CaseWorkerTaskAggregate.BuildCaseWorkerTaskIdentifier(activateCommand.CasePlanInstanceId, activateCommand.CasePlanElementInstanceId);
            var activation = await _eventStoreRepository.GetLastAggregate<CaseWorkerTaskAggregate>(id, CaseWorkerTaskAggregate.GetStreamName(id));
            if (activation == null || string.IsNullOrWhiteSpace(activation.Id))
            {
                throw new UnknownCaseActivationException();
            }

            if (activateCommand.BypassUserValidation)
            {
                activation.Confirm();
            }
            else
            {
                var roles = await _roleQueryRepository.FindRolesByUser(activateCommand.Performer);
                activation.Confirm(roles.Select(r => r.Name));
            }

            await _commitAggregateHelper.Commit(activation, CaseWorkerTaskAggregate.GetStreamName(id), CMMNConstants.QueueNames.CasePlanInstances);
            await _messageBroker.QueueTransition(activation.CasePlanInstanceId, activation.CasePlanElementInstanceId, CMMNTransitions.ManualStart);
        }
    }
}
