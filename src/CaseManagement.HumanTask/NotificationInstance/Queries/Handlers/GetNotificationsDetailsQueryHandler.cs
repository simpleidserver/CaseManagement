using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Localization;
using CaseManagement.HumanTask.NotificationInstance.Queries.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationInstance.Queries.Handlers
{
    public class GetNotificationsDetailsQueryHandler : IRequestHandler<GetNotificationsDetailsQuery, NotificationDetailsResult>
    {
        private readonly INotificationInstanceQueryRepository _notificationInstanceQueryRepository;
        private readonly ITranslationHelper _translationHelper;
        private readonly ILogger<GetNotificationsDetailsQueryHandler> _logger;

        public GetNotificationsDetailsQueryHandler(
            INotificationInstanceQueryRepository notificationInstanceQueryRepository,
            ITranslationHelper translationHelper,
            ILogger<GetNotificationsDetailsQueryHandler> logger)
        {
            _notificationInstanceQueryRepository = notificationInstanceQueryRepository;
            _translationHelper = translationHelper;
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

            var callbackTxt = new Func<ICollection<Domains.Text>, Translation>((t) =>
            {
                if (t == null || !t.Any())
                {
                    return null;
                }

                try
                {
                    return _translationHelper.Translate(t);
                }
                catch (BadOperationExceptions) { return null; }
            });
            var name = callbackTxt(result.PresentationElement.Names);
            var subject = callbackTxt(result.PresentationElement.Subjects);
            return NotificationDetailsResult.ToDto(result, name, subject);
        }
    }
}
