using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands.Handlers
{
    public class UpdateHumanTaskDefPeopleAssignmentCommandHandler : IRequestHandler<UpdateHumanTaskDefPeopleAssignmentCommand, bool>
    {
        private readonly ILogger<UpdateHumanTaskDefPeopleAssignmentCommandHandler> _logger;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly IHumanTaskDefCommandRepository _humanTaskDefCommandRepository;

        public UpdateHumanTaskDefPeopleAssignmentCommandHandler(ILogger<UpdateHumanTaskDefPeopleAssignmentCommandHandler> logger, IHumanTaskDefQueryRepository humanTaskDefQueryRepository, IHumanTaskDefCommandRepository humanTaskDefCommandRepository)
        {
            _logger = logger;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _humanTaskDefCommandRepository = humanTaskDefCommandRepository;
        }

        public async Task<bool> Handle(UpdateHumanTaskDefPeopleAssignmentCommand request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            result.UpdatePeopleAssignment(
                request.PeopleAssignment.PotentialOwner?.ToDomain(),
                request.PeopleAssignment.ExcludedOwner?.ToDomain(),
                request.PeopleAssignment.TaskInitiator?.ToDomain(),
                request.PeopleAssignment.TaskStakeHolder?.ToDomain(),
                request.PeopleAssignment.BusinessAdministrator?.ToDomain(),
                request.PeopleAssignment.Recipient?.ToDomain());
            await _humanTaskDefCommandRepository.Update(result, cancellationToken);
            await _humanTaskDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation("The people assignement has been updated");
            return true;
        }
    }
}
