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
    public class AddNotificationDefParameterCommandHandler : IRequestHandler<AddNotificationDefParameterCommand, string>
    {
        private readonly ILogger<AddNotificationDefParameterCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public AddNotificationDefParameterCommandHandler(ILogger<AddNotificationDefParameterCommandHandler> logger, INotificationDefQueryRepository notificationDefQueryRepository, INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<string> Handle(AddNotificationDefParameterCommand request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownNotification, request.Id));
            }

            if (request.Parameter == null)
            {
                _logger.LogError($"The 'parameter' parameter is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "parameter"));
            }

            var id = result.AddOperationParameter(request.Parameter.ToDomain());
            await _notificationDefCommandRepository.Update(result, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Notification  '{result.Name}', operation parameter '{request.Parameter.Name}' has been added");
            return id;
        }
    }
}
