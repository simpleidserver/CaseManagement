using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands.Handlers
{
    public class UpdateHumanTaskDefPresentationParametersCommandHandler : IRequestHandler<UpdateHumanTaskDefPresentationParametersCommand, bool>
    {
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly ILogger<UpdateHumanTaskDefPresentationParametersCommandHandler> _logger;

        public UpdateHumanTaskDefPresentationParametersCommandHandler(
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            ILogger<UpdateHumanTaskDefPresentationParametersCommandHandler> logger)
        {
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateHumanTaskDefPresentationParametersCommand request, CancellationToken cancellationToken)
        {
            if (request.PresentationElements == null)
            {
                _logger.LogError("the parameter 'presentationElements' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "presentationElements"));
            }

            if (request.PresentationParameters == null)
            {
                _logger.LogError("the parameter 'presentationParameters' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "presentationParameters"));
            }

            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            result.UpdatePresentationElts(request.PresentationElements.Select(_ => _.ToDomain()).ToList());
            result.UpdatePresentationParameters(request.PresentationParameters.Select(_ => _.ToDomain()).ToList());
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Human task definition '{result.Name}', presentation element has been updated");
            return true;
        }
    }
}
