using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.NotificationDef.Results;
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
    public class AddNotificationDefCommandHandler : IRequestHandler<AddNotificationDefCommand, NotificationDefResult>
    {
        private readonly ILogger<AddNotificationDefCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationDefCommandRepository _notificationDefCommandRepository;

        public AddNotificationDefCommandHandler(
            ILogger<AddNotificationDefCommandHandler> logger,
            INotificationDefQueryRepository notificationDefQueryRepository,
            INotificationDefCommandRepository notificationDefCommandRepository)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationDefCommandRepository = notificationDefCommandRepository;
        }

        public async Task<NotificationDefResult> Handle(AddNotificationDefCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogError("the parameter 'name' is missing");
                throw new BadRequestException(string.Format(Global.MissingParameter, "name"));
            }

            var result = await _notificationDefQueryRepository.Search(new SearchNotificationDefParameter
            {
                Name = request.Name
            }, cancellationToken);
            if (result != null && result.Content.Count() > 0)
            {
                _logger.LogError($"the notification '{request.Name}' already exists");
                throw new BadRequestException(string.Format(Global.NotificationDefExists, request.Name));
            }

            var res = NotificationDefinitionAggregate.New(request.Name);
            await _notificationDefCommandRepository.Add(res, cancellationToken);
            await _notificationDefCommandRepository.SaveChanges(cancellationToken);
            _logger.LogInformation($"the notification definition '{request.Name}' has been added");
            return NotificationDefResult.ToDto(res);
        }
    }
}
