using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.HumanTaskDef.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.Parameters;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands.Handlers
{
    public class AddHumanTaskDefCommandHandler : IRequestHandler<AddHumanTaskDefCommand, HumanTaskDefResult>
    {
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly ILogger<AddHumanTaskDefCommandHandler> _logger;

        public AddHumanTaskDefCommandHandler(
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            ILogger<AddHumanTaskDefCommandHandler> logger)
        {
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _logger = logger;
        }

        public async Task<HumanTaskDefResult> Handle(AddHumanTaskDefCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogError("the parameter 'name' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "name"));
            }

            var result = await _humanTaskDefQueryRepository.Search(new SearchHumanTaskDefParameter
            {
                Name = request.Name
            }, cancellationToken);
            if (result != null && result.Content.Count() > 0)
            {
                _logger.LogError($"the human task '{request.Name}' already exists");
                throw new BadRequestException(string.Format(Global.HumanTaskDefExists, request.Name));
            }

            var humanTaskDef = HumanTaskDefinitionAggregate.New(request.Name);
            await _humanTaskDefCommandRepository.Add(humanTaskDef, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"the human task definition '{request.Name}' has been added");
            return HumanTaskDefResult.ToDto(humanTaskDef);
        }
    }
}
