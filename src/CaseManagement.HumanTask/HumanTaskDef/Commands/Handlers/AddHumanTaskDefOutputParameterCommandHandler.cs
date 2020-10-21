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
    public class AddHumanTaskDefOutputParameterCommandHandler : IRequestHandler<AddHumanTaskDefOutputParameterCommand, bool>
    {
        private readonly ILogger<AddHumanTaskDefInputParameterCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public AddHumanTaskDefOutputParameterCommandHandler(ILogger<AddHumanTaskDefInputParameterCommandHandler> logger, IHumanTaskDefQueryRepository humanTaskDefQueryRepository, IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(AddHumanTaskDefOutputParameterCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            if (request.Parameter == null)
            {
                _logger.LogError($"The 'parameter' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "parameter"));
            }

            if (result.Operation.OutputParameters.Any(_ => _.Name == request.Parameter.Name))
            {
                _logger.LogError($"The input parameter '{request.Parameter.Name}' already exists");
                throw new BadRequestException(string.Format(Global.OutputParameterExists, "parameter"));
            }

            result.AddOutputParameter(request.Parameter.ToDomain());
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
