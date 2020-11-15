using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Resources;
using CaseManagement.Common.Bus;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Commands.Handlers
{
    public class StartProcessInstanceCommandHandler : IRequestHandler<StartProcessInstanceCommand, bool>
    {
        private readonly IProcessInstanceQueryRepository _processInstanceQueryRepository;
        private readonly ILogger<StartProcessInstanceCommandHandler> _logger;
        private readonly IMessageBroker _messageBroker;

        public StartProcessInstanceCommandHandler(
            IProcessInstanceQueryRepository processInstanceQueryRepository,
            ILogger<StartProcessInstanceCommandHandler> logger,
            IMessageBroker messageBroker)
        {
            _processInstanceQueryRepository = processInstanceQueryRepository;
            _logger = logger;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(StartProcessInstanceCommand request, CancellationToken cancellationToken)
        {
            var result = await _processInstanceQueryRepository.Get(request.ProcessInstanceId, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"unknown process instance '{request.ProcessInstanceId}'");
                throw new UnknownFlowInstanceException(string.Format(Global.UnknownProcessInstance, request.ProcessInstanceId));
            }

            await _messageBroker.QueueProcessInstance(request.ProcessInstanceId, true, cancellationToken);
            return true;
        }
    }
}
