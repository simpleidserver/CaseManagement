using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands.Handlers
{
    public class DeleteHumanTaskDefEscalationStartDeadlineCommandHandler : IRequestHandler<DeleteHumanTaskDefEscalationStartDeadlineCommand, bool>
    {
        private readonly ILogger<DeleteHumanTaskDefEscalationStartDeadlineCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public DeleteHumanTaskDefEscalationStartDeadlineCommandHandler(
            ILogger<DeleteHumanTaskDefEscalationStartDeadlineCommandHandler> logger,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(DeleteHumanTaskDefEscalationStartDeadlineCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            result.DeleteEscalationStartDeadline(request.DeadLineId, request.EscalationId);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation("Escalation has been removed from start deadline");
            return true;
        }
    }
}
