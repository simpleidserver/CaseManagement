using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.Parameters;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Commands.Handlers
{
    public class UpdateNotificationDefInfoCommandHandler : IRequestHandler<UpdateNotificationDefInfoCommand, bool>
    {
        private readonly ILogger<UpdateNotificationDefInfoCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public UpdateNotificationDefInfoCommandHandler(
            ILogger<UpdateNotificationDefInfoCommandHandler> logger, 
            INotificationDefQueryRepository notificationDefQueryRepository,
            INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<bool> Handle(UpdateNotificationDefInfoCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogError("the parameter 'name' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "name"));
            }

            var result = await _notificationDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The notification definition '{request.Id}' doesn't exist");
                throw new UnknownNotificationDefException(string.Format(Global.UnknownNotification, request.Id));
            }

            var r = await _notificationDefQueryRepository.Search(new SearchNotificationDefParameter
            {
                Name = request.Name
            }, cancellationToken);
            r.Content = r.Content.Where(_ => _.AggregateId != request.Id).ToList();
            if (r != null && r.Content.Count() > 0)
            {
                _logger.LogError($"the notification '{request.Name}' already exists");
                throw new BadRequestException(string.Format(Global.NotificationDefExists, request.Name));
            }

            result.UpdateInfo(request.Name, request.Priority);
            await _notificationDefCommandRepository.Update(result, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"Notification definition '{result.Name}', information has been updated");
            return true;
        }
    }
}
