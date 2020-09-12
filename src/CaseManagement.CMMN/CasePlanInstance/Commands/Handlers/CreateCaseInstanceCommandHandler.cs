using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.CMMN.Common.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class CreateCaseInstanceCommandHandler : IRequestHandler<CreateCaseInstanceCommand, CasePlanInstanceResult>
    {
        private readonly ICasePlanQueryRepository _casePlanQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public CreateCaseInstanceCommandHandler(ICasePlanQueryRepository casePlanQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _casePlanQueryRepository = casePlanQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<CasePlanInstanceResult> Handle(CreateCaseInstanceCommand request, CancellationToken cancellationToken)
        {
            var workflowDefinition = await _casePlanQueryRepository.Get(request.CasePlanId, cancellationToken);
            if (workflowDefinition == null)
            {
                throw new UnknownCasePlanException(request.CasePlanId);
            }

            var permissions = request.Permissions == null ? new List<CasePlanInstanceRole>() : request.Permissions.Select(_ => new CasePlanInstanceRole
            {
                Id = _.RoleId,
                Claims = _.Claims.ToList()
            }).ToList();
            var casePlanInstance = CasePlanInstanceAggregate.New(Guid.NewGuid().ToString(), workflowDefinition, permissions, request.Parameters);
            await _commitAggregateHelper.Commit(casePlanInstance, casePlanInstance.GetStreamName(), cancellationToken);
            return CasePlanInstanceResult.ToDto(casePlanInstance);
        }
    }
}
