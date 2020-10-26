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
    public class AddHumanTaskDefEscalationStartDeadlineCommandHandler : IRequestHandler<AddHumanTaskDefEscalationStartDeadlineCommand, string>
    {
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly ILogger<AddHumanTaskDefEscalationStartDeadlineCommandHandler> _logger;

        public AddHumanTaskDefEscalationStartDeadlineCommandHandler(
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            ILogger<AddHumanTaskDefEscalationStartDeadlineCommandHandler> logger)
        {
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _logger = logger;
        }

        public async Task<string> Handle(AddHumanTaskDefEscalationStartDeadlineCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Condition))
            {
                _logger.LogError("the parameter 'condition' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "condition"));
            }

            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            var id = result.AddEscalationStartDeadline(request.StartDeadlineId, request.Condition);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation("Escalation has been added to start deadline");
            return id;
        }
    }
}
