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
    public class DeleteHumanTaskDefPeopleAssignmentCommandHandler : IRequestHandler<DeleteHumanTaskDefPeopleAssignmentCommand, bool>
    {
        private readonly ILogger<DeleteHumanTaskDefPeopleAssignmentCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public DeleteHumanTaskDefPeopleAssignmentCommandHandler(
            ILogger<DeleteHumanTaskDefPeopleAssignmentCommandHandler> logger,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(DeleteHumanTaskDefPeopleAssignmentCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            if (!result.PeopleAssignments.Any(_ => _.Id == request.AssignmentId))
            {
                _logger.LogError($"People assignment '{request.AssignmentId}' doesn't exist");
                throw new BadRequestException(string.Format(Global.PeopleAssignmentDoesntExist, request.AssignmentId));
            }

            result.RemoveAssignment(request.AssignmentId);
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Human task definition '{result.Name}', people assignment is removed");
            return true;
        }
    }
}
