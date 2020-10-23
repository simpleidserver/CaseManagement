using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
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
    public class UpdateHumanTaskDefInfoCommandHandler : IRequestHandler<UpdateHumanTaskDefInfoCommand, bool>
    {
        private readonly ILogger<UpdateHumanTaskDefInfoCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public UpdateHumanTaskDefInfoCommandHandler(ILogger<UpdateHumanTaskDefInfoCommandHandler> logger, IHumanTaskDefQueryRepository humanTaskDefQueryRepository, IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(UpdateHumanTaskDefInfoCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogError("the parameter 'name' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "name"));
            }

            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            var r = await _humanTaskDefQueryRepository.Search(new SearchHumanTaskDefParameter
            {
                Name = request.Name
            }, cancellationToken);
            r.Content = r.Content.Where(_ => _.AggregateId != request.Id).ToList();
            if (r != null && r.Content.Count() > 0)
            {
                _logger.LogError($"the human task '{request.Name}' already exists");
                throw new BadRequestException(string.Format(Global.HumanTaskDefExists, request.Name));
            }

            result.UpdateInfo(request.Name, request.Priority);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Human task definition '{result.Name}', information has been updated");
            return true;
        }
    }
}
