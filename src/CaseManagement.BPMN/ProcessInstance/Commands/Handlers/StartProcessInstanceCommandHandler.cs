using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Processors;
using CaseManagement.BPMN.Resources;
using MassTransit;
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
        private readonly IBusControl _busControl;
        private readonly ILogger<StartProcessInstanceCommandHandler> _logger;

        public StartProcessInstanceCommandHandler(
            IProcessInstanceCommandRepository processInstanceCommandRepository,
            IProcessInstanceProcessor processInstanceProcessor,
            IBusControl busControl,
            ILogger<StartProcessInstanceCommandHandler> logger)
        {
            _processInstanceCommandRepository = processInstanceCommandRepository;
            _processInstanceProcessor = processInstanceProcessor;
            _busControl = busControl;
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
            if (request.NewExecutionPath)
            {
                processInstance.NewExecutionPath();
            }

            var isRestarted = await _processInstanceProcessor.Execute(processInstance, cancellationToken);
            if (isRestarted)
            {
                var evt = processInstance.Restart();
                await _processInstanceCommandRepository.Update(processInstance, cancellationToken);
                await _processInstanceCommandRepository.SaveChanges(cancellationToken);
                await _busControl.Publish(evt, cancellationToken);
            }

            await _processInstanceCommandRepository.Update(processInstance, cancellationToken);
            await _processInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
