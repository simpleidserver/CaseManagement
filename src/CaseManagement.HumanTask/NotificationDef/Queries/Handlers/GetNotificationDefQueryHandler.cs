using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskDef.Results;
using CaseManagement.HumanTask.NotificationDef.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Queries.Handlers
{
    public class GetNotificationDefQueryHandler : IRequestHandler<GetNotificationDefQuery, NotificationDefResult>
    {
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly ILogger<GetNotificationDefQueryHandler> _logger;

        public GetNotificationDefQueryHandler(
            INotificationDefQueryRepository notificationDefQueryRepository,
            ILogger<GetNotificationDefQueryHandler> logger)
        {
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _logger = logger;
        }

        public async Task<NotificationDefResult> Handle(GetNotificationDefQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            return NotificationDefResult.ToDto(result);
        }
    }
}
