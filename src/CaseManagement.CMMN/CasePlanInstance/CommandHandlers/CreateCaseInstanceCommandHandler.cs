using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using System;
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

        public async Task<CasePlanInstanceAggregate> Handle(CreateCaseInstanceCommand command, CancellationToken token)
        {
            var workflowDefinition = await _casePlanQueryRepository.Get(command.CasePlanId, token);
            if (workflowDefinition == null)
            {
                throw new UnknownCaseDefinitionException();
            }

            var workflowInstance = CasePlanInstanceAggregate.New(Guid.NewGuid().ToString(), workflowDefinition);
            await _commitAggregateHelper.Commit(workflowInstance, workflowInstance.GetStreamName(), token);
            return workflowInstance;
        }
    }
}