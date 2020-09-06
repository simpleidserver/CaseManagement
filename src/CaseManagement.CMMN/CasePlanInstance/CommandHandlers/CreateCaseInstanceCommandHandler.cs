using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public class CreateCaseInstanceCommandHandler : ICreateCaseInstanceCommandHandler
    {
        private readonly ICasePlanQueryRepository _casePlanQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public CreateCaseInstanceCommandHandler(ICasePlanQueryRepository casePlanQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _casePlanQueryRepository = casePlanQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<Domains.CasePlanInstanceAggregate> Handle(CreateCaseInstanceCommand command, CancellationToken token)
        {
            var workflowDefinition = await _casePlanQueryRepository.Get(command.CasePlanId, token);
            if (workflowDefinition == null)
            {
                throw new UnknownCaseDefinitionException();
            }

            if (!command.BypassUserValidation)
            {
                if (workflowDefinition.CaseOwner != command.Performer)
                {
                    throw new UnauthorizedCaseWorkerException(command.Performer, null, null);
                }
            }

            /*
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            await _commitAggregateHelper.Commit(workflowInstance, workflowInstance.GetStreamName(), CMMNConstants.QueueNames.CasePlanInstances);
            return workflowInstance;
            */
            return null;
        }
    }
}