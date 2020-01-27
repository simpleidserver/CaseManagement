using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure.Bus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class ConfirmTableItemCommandHandler : IConfirmTableItemCommandHandler
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly ICaseDefinitionQueryRepository _caseDefinitionQueryRepository;
        private readonly IQueueProvider _queueProvider;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ConfirmTableItemCommandHandler(IEventStoreRepository eventStoreRepository, IRoleQueryRepository roleQueryRepository, ICaseDefinitionQueryRepository caseDefinitionQueryRepository, IQueueProvider queueProvider, ICommitAggregateHelper commitAggregateHelper)
        {
            _eventStoreRepository = eventStoreRepository;
            _roleQueryRepository = roleQueryRepository;
            _caseDefinitionQueryRepository = caseDefinitionQueryRepository;
            _queueProvider = queueProvider;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task Handle(ConfirmTableItemCommand confirmPlanItemCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CaseInstance>(confirmPlanItemCommand.CaseInstanceId, Domains.CaseInstance.GetStreamName(confirmPlanItemCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(confirmPlanItemCommand.CaseInstanceId);
            }

            var caseDefinition = await _caseDefinitionQueryRepository.FindById(caseInstance.CaseDefinitionId);
            var caseElementDef = caseDefinition.GetElement(confirmPlanItemCommand.CaseElementDefinitionId);
            if (caseElementDef == null)
            {
                throw new UnknownCaseElementDefinitionException(caseInstance.Id, confirmPlanItemCommand.CaseElementDefinitionId);
            }

            if (caseElementDef.TableItem != null)
            {
                if (!string.IsNullOrWhiteSpace(caseElementDef.TableItem.AuthorizedRoleRef))
                {
                    var roles = await _roleQueryRepository.FindRolesByUser(confirmPlanItemCommand.User);
                    if (!roles.Any(r => r.Id == caseElementDef.TableItem.AuthorizedRoleRef))
                    {
                        throw new UnauthorizedCaseWorkerException(confirmPlanItemCommand.User, caseInstance.Id, confirmPlanItemCommand.CaseElementDefinitionId);
                    }
                }
            }

            caseInstance.ConfirmTableItem(confirmPlanItemCommand.CaseElementDefinitionId, confirmPlanItemCommand.User);
            if (caseInstance.IsRunning())
            {
                await _queueProvider.QueueConfirmTableItem(caseInstance.Id, confirmPlanItemCommand.CaseElementDefinitionId, confirmPlanItemCommand.User);
            }
            else
            {
                await _commitAggregateHelper.Commit(caseInstance, new List<DomainEvent> { caseInstance.DomainEvents.Last() }, caseInstance.Version, caseInstance.GetStreamName());
            }
        }
    }
}
