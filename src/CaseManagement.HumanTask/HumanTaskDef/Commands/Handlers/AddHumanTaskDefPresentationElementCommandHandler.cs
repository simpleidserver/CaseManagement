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
    public class AddHumanTaskDefPresentationElementCommandHandler : IRequestHandler<AddHumanTaskDefPresentationElementCommand, bool>
    {
        private readonly ILogger<AddHumanTaskDefPresentationElementCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public AddHumanTaskDefPresentationElementCommandHandler(
            ILogger<AddHumanTaskDefPresentationElementCommandHandler> logger, 
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository, 
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(AddHumanTaskDefPresentationElementCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            if (request.PresentationElement == null)
            {
                _logger.LogError($"The 'presentationElement' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "parameter"));
            }

            var record = request.PresentationElement.ToDomain();
            if (result.PresentationElements.Any(_ => _.Usage == record.Usage && _.Language == record.Language))
            {
                _logger.LogError($"The presentation element already exists");
                throw new BadRequestException(Global.PresentationElementExists);
            }

            result.AddPresentationElement(record);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Human task definition '{result.Name}', presentation element has been added");
            return true;
        }
    }
}
