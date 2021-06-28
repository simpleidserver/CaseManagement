using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.CMMN.Common.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using MassTransit;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class CreateCaseInstanceCommandHandler : IRequestHandler<CreateCaseInstanceCommand, CasePlanInstanceResult>
    {
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;
        private readonly ICasePlanInstanceCommandRepository _casePlanInstanceCommandRepository;
        private readonly IBusControl _busControl;

        public CreateCaseInstanceCommandHandler(
            ICasePlanCommandRepository casePlanCommandRepository,
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            IBusControl busControl)
        {
            _casePlanCommandRepository = casePlanCommandRepository;
            _casePlanInstanceCommandRepository = casePlanInstanceCommandRepository;
            _busControl = busControl;
        }

        public async Task<CasePlanInstanceResult> Handle(CreateCaseInstanceCommand request, CancellationToken cancellationToken)
        {
            var workflowDefinition = await _casePlanCommandRepository.Get(request.CasePlanId, cancellationToken);
            if (workflowDefinition == null)
            {
                throw new UnknownCasePlanException(request.CasePlanId);
            }

            var casePlanInstance = CasePlanInstanceAggregate.New(Guid.NewGuid().ToString(), workflowDefinition, request.NameIdentifier, request.Parameters);
            await _casePlanInstanceCommandRepository.Add(casePlanInstance, cancellationToken);
            await _casePlanInstanceCommandRepository.SaveChanges(cancellationToken);
            await _busControl.Publish(casePlanInstance.DomainEvents.First(s => s is CasePlanInstanceCreatedEvent) as CasePlanInstanceCreatedEvent, cancellationToken);
            return CasePlanInstanceResult.ToDto(casePlanInstance);
        }
    }
}
