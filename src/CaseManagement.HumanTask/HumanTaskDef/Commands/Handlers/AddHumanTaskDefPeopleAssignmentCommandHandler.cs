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
    public class AddHumanTaskDefPeopleAssignmentCommandHandler : IRequestHandler<AddHumanTaskDefPeopleAssignmentCommand, string>
    {
        private readonly ILogger<AddHumanTaskDefPeopleAssignmentCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public AddHumanTaskDefPeopleAssignmentCommandHandler(
            ILogger<AddHumanTaskDefPeopleAssignmentCommandHandler> logger, 
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository, 
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<string> Handle(AddHumanTaskDefPeopleAssignmentCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            if (request.PeopleAssignment == null)
            {
                _logger.LogError($"The 'peopleAssignment' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "peopleAssignment"));
            }

            var id = result.Assign(request.PeopleAssignment.ToDomain());
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Human task definition '{result.Name}', people is assigned");
            return id;
        }
    }
}
