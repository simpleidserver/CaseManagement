using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands.Handlers
{
    public class DeleteHumanTaskDefStartDeadlineCommandHandler : IRequestHandler<DeleteHumanTaskDefStartDeadlineCommand, bool>
    {
        private readonly ILogger<DeleteHumanTaskDefStartDeadlineCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public DeleteHumanTaskDefStartDeadlineCommandHandler(
            ILogger<DeleteHumanTaskDefStartDeadlineCommandHandler> logger,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(DeleteHumanTaskDefStartDeadlineCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            result.DeleteStartDeadLine(request.DeadLineId);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
