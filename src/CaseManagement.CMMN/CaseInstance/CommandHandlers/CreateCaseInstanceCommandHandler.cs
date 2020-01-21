using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class CreateCaseInstanceCommandHandler : ICreateCaseInstanceCommandHandler
    {
        private readonly ICaseDefinitionQueryRepository _cmmnWorkflowDefinitionQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public CreateCaseInstanceCommandHandler(ICaseDefinitionQueryRepository cmmnWorkflowDefinitionQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _cmmnWorkflowDefinitionQueryRepository = cmmnWorkflowDefinitionQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<Domains.CaseInstance> Handle(CreateCaseInstanceCommand command)
        {
            var workflowDefinition = await _cmmnWorkflowDefinitionQueryRepository.FindById(command.CaseDefinitionId);
            if (workflowDefinition == null)
            {
                throw new UnknownCaseDefinitionException();
            }
            
            var workflowInstance = Domains.CaseInstance.New(workflowDefinition);
            await _commitAggregateHelper.Commit(workflowInstance, workflowInstance.GetStreamName());
            return workflowInstance;
        }
    }
}
