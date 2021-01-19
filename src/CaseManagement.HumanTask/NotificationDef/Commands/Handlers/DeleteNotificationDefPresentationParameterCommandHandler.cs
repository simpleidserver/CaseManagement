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
    public class DeleteNotificationDefPresentationParameterCommandHandler : IRequestHandler<DeleteNotificationDefPresentationParameterCommand, bool>
    {
        private readonly ILogger<DeleteNotificationDefPresentationParameterCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public DeleteNotificationDefPresentationParameterCommandHandler(ILogger<DeleteNotificationDefPresentationParameterCommandHandler> logger, INotificationDefQueryRepository notificationDefQueryRepository, INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<bool> Handle(DeleteNotificationDefPresentationParameterCommand request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownNotificationDefException(string.Format(Global.UnknownNotification, request.Id));
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogError($"The 'name' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "name"));
            }

            result.DeletePresentationParameter(request.Name);
            await _notificationDefCommandRepository.Update(result, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Notification definition '{result.AggregateId}', presentation parameter '{request.Name}' has been removed");
            return true;
        }
    }
}
