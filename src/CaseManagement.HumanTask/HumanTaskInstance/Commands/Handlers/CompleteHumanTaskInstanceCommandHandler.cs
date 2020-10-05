using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands.Handlers
{
    public class CompleteHumanTaskInstanceCommandHandler : IRequestHandler<CompleteHumanTaskInstanceCommand, bool>
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IParameterParser _parameterParser;
        private readonly ILogger<CompleteHumanTaskInstanceCommandHandler> _logger;

        public CompleteHumanTaskInstanceCommandHandler(
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IParameterParser parameteParser,
            ILogger<CompleteHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _parameterParser = parameteParser;
            _logger = logger;
        }

        public async Task<bool> Handle(CompleteHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            if (request.OperationParameters == null || !request.OperationParameters.Any())
            {
                _logger.LogError("Output data must be specified");
                throw new BadRequestException(string.Format(Global.MissingParameter, "operationParameters"));
            }

            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            if (humanTaskInstance.Status != HumanTaskInstanceStatus.INPROGRESS)
            {
                _logger.LogError("Complete operation can be performed only on INPROGRESS human task instance");
                throw new BadOperationExceptions(string.Format(Global.OperationCanBePerformed, "Complete", "INPROGRESS"));
            }

            var operationParameters = request.OperationParameters;
            var parameters = _parameterParser.ParseOperationParameters(humanTaskInstance.Operation.OutputParameters, operationParameters);
            var nameIdentifier = request.Claims.GetUserNameIdentifier();
            if (nameIdentifier != humanTaskInstance.ActualOwner)
            {
                _logger.LogError("Authenticated user is not the actual owner");
                throw new NotAuthorizedException(Global.NotActualOwner);
            }

            humanTaskInstance.Complete(parameters, nameIdentifier);
            await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
