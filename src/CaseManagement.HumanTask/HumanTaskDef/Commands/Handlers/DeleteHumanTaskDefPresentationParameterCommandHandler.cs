using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands.Handlers
{
    public class DeleteHumanTaskDefPresentationParameterCommandHandler : IRequestHandler<DeleteHumanTaskDefPresentationParameterCommand, bool>
    {
        private readonly ILogger<DeleteHumanTaskDefPresentationParameterCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public DeleteHumanTaskDefPresentationParameterCommandHandler(ILogger<DeleteHumanTaskDefPresentationParameterCommandHandler> logger, IHumanTaskDefQueryRepository humanTaskDefQueryRepository, IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(DeleteHumanTaskDefPresentationParameterCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogError($"The 'name' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "name"));
            }

            if (!result.PresentationParameters.Any(_ => _.Name == request.Name))
            {
                _logger.LogError($"The presentation parameter '{request.Name}' doesn't exist");
                throw new BadRequestException(string.Format(Global.PresentationParameterDoesntExist, request.Name));
            }

            result.DeletePresentationParameter(request.Name);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Human task definition '{result.AggregateId}', presentation parameter '{request.Name}' has been removed");
            return true;
        }
    }
}
