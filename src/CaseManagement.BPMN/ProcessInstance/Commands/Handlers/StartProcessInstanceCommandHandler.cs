using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Resources;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Commands.Handlers
{
    public class StartProcessInstanceCommandHandler : IRequestHandler<StartProcessInstanceCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ILogger<StartProcessInstanceCommandHandler> _logger;
        private readonly IMessageBroker _messageBroker;

        public StartProcessInstanceCommandHandler(
            IEventStoreRepository eventStoreRepository,
            ILogger<StartProcessInstanceCommandHandler> logger,
            IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _logger = logger;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(StartProcessInstanceCommand request, CancellationToken cancellationToken)
        {
            var processInstance = await _eventStoreRepository.GetLastAggregate<ProcessInstanceAggregate>(request.ProcessInstanceId, ProcessInstanceAggregate.GetStreamName(request.ProcessInstanceId));
            if (request == null || string.IsNullOrWhiteSpace(processInstance.AggregateId))
            {
                _logger.LogError($"unknown process instance '{request.ProcessInstanceId}'");
                throw new UnknownFlowInstanceException(string.Format(Global.UnknownProcessInstance, request.ProcessInstanceId));
            }

            await _messageBroker.QueueProcessInstance(request.ProcessInstanceId, true, cancellationToken);
            return true;
        }
    }
}
