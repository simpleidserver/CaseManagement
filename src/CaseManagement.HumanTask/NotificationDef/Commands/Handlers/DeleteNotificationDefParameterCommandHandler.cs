using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Commands.Handlers
{
    public class DeleteNotificationDefParameterCommandHandler : IRequestHandler<DeleteNotificationDefParameterCommand, bool>
    {
        private readonly ILogger<DeleteNotificationDefParameterCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public DeleteNotificationDefParameterCommandHandler(ILogger<DeleteNotificationDefParameterCommandHandler> logger, INotificationDefQueryRepository notificationDefQueryRepository, INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<bool> Handle(DeleteNotificationDefParameterCommand request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownNotificationDefException(string.Format(Global.UnknownNotification, request.Id));
            }

            result.DeleteOperationParameter(request.ParameterId);
            await _notificationDefCommandRepository.Update(result, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Notification definition '{result.Name}', operation parameter '{request.ParameterId}' has been removed");
            return true;
        }
    }
}
