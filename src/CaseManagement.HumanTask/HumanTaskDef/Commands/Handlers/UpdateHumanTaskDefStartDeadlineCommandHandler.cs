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
    public class UpdateHumanTaskDefStartDeadlineCommandHandler : IRequestHandler<UpdateHumanTaskDefStartDeadlineCommand, bool>
    {
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly ILogger<UpdateHumanTaskDefStartDeadlineCommandHandler> _logger;

        public UpdateHumanTaskDefStartDeadlineCommandHandler(
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            ILogger<UpdateHumanTaskDefStartDeadlineCommandHandler> logger)
        {
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateHumanTaskDefStartDeadlineCommand request, CancellationToken cancellationToken)
        {
            if (request.DeadLineInfo == null)
            {
                _logger.LogError("the parameter 'deadLineInfo' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "deadLineInfo"));
            }

            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            result.UpdateStartDeadline(request.DeadLineId,
                request.DeadLineInfo.Name,
                request.DeadLineInfo.For,
                request.DeadLineInfo.Until);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}