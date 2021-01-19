using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Commands.Handlers
{
    public class DeleteNotificationDefPresentationElementCommandHandler : IRequestHandler<DeleteNotificationDefPresentationElementCommand, bool>
    {
        private readonly ILogger<DeleteNotificationDefPresentationElementCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public DeleteNotificationDefPresentationElementCommandHandler(
            ILogger<DeleteNotificationDefPresentationElementCommandHandler> logger,
            INotificationDefQueryRepository notificationDefQueryRepository,
            INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<bool> Handle(DeleteNotificationDefPresentationElementCommand request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownNotificationException(string.Format(Global.UnknownNotification, request.Id));
            }

            result.DeletePresentationElement(request.Usage, request.Language);
            await _notificationDefCommandRepository.Update(result, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Notification definition '{result.Name}', presentation element has been remvoed");
            return true;
        }
    }
}
