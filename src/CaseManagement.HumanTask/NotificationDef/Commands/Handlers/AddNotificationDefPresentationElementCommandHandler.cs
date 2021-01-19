using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Commands.Handlers
{
    public class AddNotificationDefPresentationElementCommandHandler : IRequestHandler<AddNotificationDefPresentationElementCommand, bool>
    {
        private readonly ILogger<AddNotificationDefPresentationElementCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public AddNotificationDefPresentationElementCommandHandler(
            ILogger<AddNotificationDefPresentationElementCommandHandler> logger, 
            INotificationDefQueryRepository notificationDefQueryRepository,
            INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<bool> Handle(AddNotificationDefPresentationElementCommand request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownNotification, request.Id));
            }

            if (request.PresentationElement == null)
            {
                _logger.LogError($"The 'presentationElement' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "parameter"));
            }

            result.AddPresentationElement(request.PresentationElement.ToDomain());
            await _notificationDefCommandRepository.Update(result, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Notification definition '{result.Name}', presentation element has been added");
            return true;
        }
    }
}
