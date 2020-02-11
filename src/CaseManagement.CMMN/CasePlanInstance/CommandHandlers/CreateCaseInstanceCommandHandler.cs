using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class CreateCaseInstanceCommandHandler : ICreateCaseInstanceCommandHandler
    {
        private readonly ICasePlanQueryRepository _cmmnWorkflowDefinitionQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public CreateCaseInstanceCommandHandler(ICasePlanQueryRepository cmmnWorkflowDefinitionQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _cmmnWorkflowDefinitionQueryRepository = cmmnWorkflowDefinitionQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<Domains.CasePlanInstanceAggregate> Handle(CreateCaseInstanceCommand command)
        {
            var workflowDefinition = await _cmmnWorkflowDefinitionQueryRepository.FindById(command.CasePlanId);
            if (workflowDefinition == null)
            {
                throw new UnknownCaseDefinitionException();
            }

            if (workflowDefinition.CaseOwner != command.NameIdentifier)
            {
                throw new UnauthorizedCaseWorkerException(command.NameIdentifier, null, null);
            }
            
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowInstance.CaseOwner = command.NameIdentifier;
            await _commitAggregateHelper.Commit(workflowInstance, workflowInstance.GetStreamName(), CMMNConstants.QueueNames.CasePlanInstances);
            return workflowInstance;
        }
    }
}