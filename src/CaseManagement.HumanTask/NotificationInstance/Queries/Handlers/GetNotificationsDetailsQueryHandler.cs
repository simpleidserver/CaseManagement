using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.NotificationInstance.Queries.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationInstance.Queries.Handlers
{
    public class GetNotificationsDetailsQueryHandler : IRequestHandler<GetNotificationsDetailsQuery, NotificationDetailsResult>
    {
        private readonly INotificationInstanceQueryRepository _notificationInstanceQueryRepository;
        private readonly ILogger<GetNotificationsDetailsQueryHandler> _logger;

        public GetNotificationsDetailsQueryHandler(
            INotificationInstanceQueryRepository notificationInstanceQueryRepository,
            ILogger<GetNotificationsDetailsQueryHandler> logger)
        {
            _notificationInstanceQueryRepository = notificationInstanceQueryRepository;
            _logger = logger;
        }

        public async Task<NotificationDetailsResult> Handle(GetNotificationsDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationInstanceQueryRepository.Get(request.NotificationId, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"Notification '{request.NotificationId}' doesn't exist");
                throw new UnknownNotificationException(string.Format(Global.UnknownNotification, request.NotificationId));
            }

            return NotificationDetailsResult.ToDto(result);
        }
    }
}
