using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands.Handlers
{
    public class UpdateHumanTaskDefEscalationCompletionDeadlineCommandHandler : IRequestHandler<UpdateHumanTaskDefEscalationCompletionDeadlineCommand, bool>
    {
        private readonly ILogger<UpdateHumanTaskDefEscalationCompletionDeadlineCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public UpdateHumanTaskDefEscalationCompletionDeadlineCommandHandler(
            ILogger<UpdateHumanTaskDefEscalationCompletionDeadlineCommandHandler> logger, 
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(UpdateHumanTaskDefEscalationCompletionDeadlineCommand request, CancellationToken cancellationToken)
        {
            if (request.Escalation == null)
            {
                _logger.LogError("the parameter 'escalation' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "escalation"));
            }

            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            result.UpdateEscalationCompletionDeadline(request.DeadLineId, request.EscalationId, request.Escalation.ToDomain());
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation("Escalation has been updated in the completion deadline");
            return true;
        }
    }
}
