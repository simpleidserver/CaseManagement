using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
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
        private readonly INotificationInstanceCommandRepository _notificationInstanceCommandRepository;
        private readonly IParameterParser _parameterParser;

        public CreateNotificationInstanceCommandHandler(
            ILogger<CreateNotificationInstanceCommandHandler> logger,
            INotificationInstanceCommandRepository notificationInstanceCommandRepository,
            IParameterParser parameterParser)
        {
            _logger = logger;
            _notificationInstanceCommandRepository = notificationInstanceCommandRepository;
            _parameterParser = parameterParser;
        }

        public async Task<string> Handle(CreateNotificationInstanceCommand request, CancellationToken cancellationToken)
        {
            var operationParameters = request.Parameters == null ? new Dictionary<string, string>() : request.Parameters;
            var parameters = _parameterParser.ParseOperationParameters(request.NotificationDef.Operation.InputParameters, operationParameters);
            _logger.LogInformation($"Create notification '{request.NotificationDef.Name}'");
            var presentationElt = _parameterParser.ParsePresentationElement(request.NotificationDef.PresentationElement, parameters);
            var assignmentInstance = _parameterParser.ParseNotificationInstancePeopleAssignment(request.NotificationDef.PeopleAssignment, parameters);
            var id = Guid.NewGuid().ToString();
            var notificationInstance = NotificationInstanceAggregate.New(id,
                request.NotificationDef.Name,
                parameters,
                presentationElt,
                assignmentInstance,
                request.NotificationDef.Rendering);
            await _notificationInstanceCommandRepository.Add(notificationInstance, cancellationToken);
            await _notificationInstanceCommandRepository.SaveChanges(cancellationToken);
            return notificationInstance.AggregateId;
        }
    }
}
