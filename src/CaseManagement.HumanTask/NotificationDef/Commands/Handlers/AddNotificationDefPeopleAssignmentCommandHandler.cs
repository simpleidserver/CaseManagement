using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Commands.Handlers
{
    public class AddNotificationDefPeopleAssignmentCommandHandler : IRequestHandler<AddNotificationDefPeopleAssignmentCommand, string>
    {
        private readonly ILogger<AddNotificationDefPeopleAssignmentCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public AddNotificationDefPeopleAssignmentCommandHandler(
            ILogger<AddNotificationDefPeopleAssignmentCommandHandler> logger,
            INotificationDefQueryRepository notificationDefQueryRepository,
            INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<string> Handle(AddNotificationDefPeopleAssignmentCommand request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownNotification, request.Id));
            }

            if (request.PeopleAssignment == null)
            {
                _logger.LogError($"The 'peopleAssignment' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "peopleAssignment"));
            }

            var id = result.Assign(request.PeopleAssignment.ToDomain());
            await _notificationDefCommandRepository.Update(result, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Notification definition '{result.Name}', people is assigned");
            return id;
        }
    }
}
