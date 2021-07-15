using CaseManagement.Common.Exceptions;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands.Handlers
{
    public class InstantiateSubTaskCommandHandler : IRequestHandler<InstantiateSubTaskCommand, string>
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IParameterParser _parameterParser;
        private readonly IMediator _mediator;
        private readonly ILogger<InstantiateSubTaskCommandHandler> _logger;

        public InstantiateSubTaskCommandHandler(
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IParameterParser parameterParser,
            IMediator mediator,
            ILogger<InstantiateSubTaskCommandHandler> logger)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _parameterParser = parameterParser;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<string> Handle(InstantiateSubTaskCommand request, CancellationToken cancellationToken)
        {
            if (request.Claims == null || !request.Claims.Any())
            {
                _logger.LogError("User is not authenticated");
                throw new NotAuthenticatedException(Global.UserNotAuthenticated);
            }

            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            var subTask = humanTaskInstance.SubTasks.FirstOrDefault(_ => _.HumanTaskName == request.SubTaskName);
            if (subTask == null)
            {
                _logger.LogError($"'{request.SubTaskName}' is not a subtask of '{humanTaskInstance.HumanTaskDefName}'");
                throw new BadRequestException(string.Format(Global.NotSubTask, request.SubTaskName, humanTaskInstance.HumanTaskDefName));
            }

            var subTasks = await _humanTaskInstanceQueryRepository.GetSubTasks(humanTaskInstance.AggregateId, cancellationToken);
            if (subTasks.Any(_ => _.HumanTaskDefName == request.SubTaskName))
            {
                _logger.LogError($"The sub task '{request.SubTaskName}' is already created");
                throw new BadRequestException(string.Format(Global.SubTaskAlreadyCreated, request.SubTaskName));
            }

            var operationParameters = _parameterParser.ParseToPartParameters(subTask.ToParts, humanTaskInstance.InputParameters);
            var result = await _mediator.Send(new CreateHumanTaskInstanceCommand 
            { 
                Claims = request.Claims,
                HumanTaskName = request.SubTaskName,
                OperationParameters = operationParameters,
                IgnorePermissions = true
            }, cancellationToken);
            var hi = await _humanTaskInstanceQueryRepository.Get(result.Id, cancellationToken);
            hi.SetParent(humanTaskInstance.HumanTaskDefName, humanTaskInstance.AggregateId);
            await _humanTaskInstanceCommandRepository.Update(hi, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            return result.Id;
        }
    }
}
