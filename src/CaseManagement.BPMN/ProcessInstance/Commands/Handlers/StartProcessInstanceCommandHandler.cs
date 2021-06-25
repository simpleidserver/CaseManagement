using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Processors;
using CaseManagement.BPMN.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Commands.Handlers
{
    public class StartProcessInstanceCommandHandler : IRequestHandler<StartProcessInstanceCommand, bool>
    {
        private readonly IProcessInstanceCommandRepository _processInstanceCommandRepository;
        private readonly IProcessInstanceProcessor _processInstanceProcessor;
        private readonly ILogger<StartProcessInstanceCommandHandler> _logger;

        public StartProcessInstanceCommandHandler(
            IProcessInstanceCommandRepository processInstanceCommandRepository,
            IProcessInstanceProcessor processInstanceProcessor,
            ILogger<StartProcessInstanceCommandHandler> logger)
        {
            _processInstanceCommandRepository = processInstanceCommandRepository;
            _processInstanceProcessor = processInstanceProcessor;
            _logger = logger;
        }

        public async Task<bool> Handle(StartProcessInstanceCommand request, CancellationToken cancellationToken)
        {
            var processInstance = await _processInstanceCommandRepository.Get(request.ProcessInstanceId, cancellationToken);
            if (request == null || string.IsNullOrWhiteSpace(processInstance.AggregateId))
            {
                _logger.LogError($"unknown process instance '{request.ProcessInstanceId}'");
                throw new UnknownFlowInstanceException(string.Format(Global.UnknownProcessInstance, request.ProcessInstanceId));
            }

            processInstance.Start(request.NameIdentifier);
            processInstance.NewExecutionPath();
            await _processInstanceProcessor.Execute(processInstance, cancellationToken);
            await _processInstanceCommandRepository.Update(processInstance, cancellationToken);
            await _processInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
