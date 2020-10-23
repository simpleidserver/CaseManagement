using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
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
            if (request.PresentationElement == null)
            {
                _logger.LogError("the parameter 'presentationElement' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "presentationElement"));
            }

            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            result.UpdatePresentationElt(request.PresentationElement.ToDomain());
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Human task definition '{result.Name}', presentation element has been updated");
            return true;
        }
    }
}
