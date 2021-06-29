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
    public class ConsumeMessageProcessInstanceCommandHandler : IRequestHandler<ConsumeMessageInstanceCommand, bool>
    {
        private readonly IProcessInstanceCommandRepository _processInstanceCommandRepository;
        private readonly IProcessInstanceProcessor _processInstanceProcessor;
        private readonly ILogger<ConsumeMessageProcessInstanceCommandHandler> _logger;

        public ConsumeMessageProcessInstanceCommandHandler(
            IProcessInstanceCommandRepository processInstanceCommandRepository,
            IProcessInstanceProcessor processInstanceProcessor,
            ILogger<ConsumeMessageProcessInstanceCommandHandler> logger)
        {
            _processInstanceCommandRepository = processInstanceCommandRepository;
            _processInstanceProcessor = processInstanceProcessor;
            _logger = logger;
        }

        public async Task<bool> Handle(ConsumeMessageInstanceCommand request, CancellationToken cancellationToken)
        {
            var processInstance = await _processInstanceCommandRepository.Get(request.FlowNodeInstanceId, cancellationToken);
            if (processInstance == null)
            {
                _logger.LogError($"unknown process instance '{request.FlowNodeInstanceId}'");
                throw new UnknownFlowInstanceException(string.Format(Global.UnknownProcessInstance, request.FlowNodeInstanceId));
            }

            processInstance.ConsumeMessage(new Domains.MessageToken
            {
                Name = request.Name,
                MessageContent = request.MessageContent.ToString()
            });
            await _processInstanceProcessor.Execute(processInstance, cancellationToken);
            await _processInstanceCommandRepository.Update(processInstance, cancellationToken);
            await _processInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
