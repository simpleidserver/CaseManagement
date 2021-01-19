using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationInstance.Commands.Handlers
{
    public class CreateNotificationInstanceCommandHandler : IRequestHandler<CreateNotificationInstanceCommand, string>
    {
        private readonly ILogger<CreateNotificationInstanceCommandHandler> _logger;
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;
        private readonly INotificationInstanceCommandRepository _notificationInstanceCommandRepository;
        private readonly IParameterParser _parameterParser;

        public CreateNotificationInstanceCommandHandler(
            ILogger<CreateNotificationInstanceCommandHandler> logger,
            INotificationDefQueryRepository notificationDefQueryRepository,
            INotificationInstanceCommandRepository notificationInstanceCommandRepository,
            IParameterParser parameterParser)
        {
            _logger = logger;
            _notificationDefQueryRepository = notificationDefQueryRepository;
            _notificationInstanceCommandRepository = notificationInstanceCommandRepository;
            _parameterParser = parameterParser;
        }

        public async Task<string> Handle(CreateNotificationInstanceCommand request, CancellationToken cancellationToken)
        {
            var notificationDef = await _notificationDefQueryRepository.Get(request.NotificationId, cancellationToken);
            if (notificationDef == null)
            {
                throw new UnknownNotificationException(string.Format(Global.UnknownNotification, request.NotificationId));
            }

            var operationParameters = request.Parameters == null ? new Dictionary<string, string>() : request.Parameters;
            var parameters = _parameterParser.ParseOperationParameters(notificationDef.InputParameters, operationParameters);
            _logger.LogInformation($"Create notification '{notificationDef.Name}'");
            var presentationElt = _parameterParser.ParsePresentationElements(notificationDef.PresentationElements, 
                notificationDef.PresentationParameters,
                parameters);
            var assignmentInstance = _parameterParser.ParsePeopleAssignments(
                notificationDef.PeopleAssignments,
                parameters);
            var id = Guid.NewGuid().ToString();
            var notificationInstance = NotificationInstanceAggregate.New(id,
                notificationDef.Priority,
                notificationDef.Name,
                parameters,
                presentationElt,
                assignmentInstance,
                notificationDef.Rendering);
            await _notificationInstanceCommandRepository.Add(notificationInstance, cancellationToken);
            await _notificationInstanceCommandRepository.SaveChanges(cancellationToken);
            return notificationInstance.AggregateId;
        }
    }
}
