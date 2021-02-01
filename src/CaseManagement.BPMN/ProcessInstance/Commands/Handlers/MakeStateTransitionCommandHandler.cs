using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Resources;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Commands.Handlers
{
    public class MakeStateTransitionCommandHandler : IRequestHandler<MakeStateTransitionCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ILogger<MakeStateTransitionCommandHandler> _logger;
        private readonly IMessageBroker _messageBroker;

        public MakeStateTransitionCommandHandler(
            IEventStoreRepository eventStoreRepository,
            ILogger<MakeStateTransitionCommandHandler> logger,
            IMessageBroker messageBroker)
        {
            _eventStoreRepository = eventStoreRepository;
            _logger = logger;
            _messageBroker = messageBroker;
        }

        public async Task<bool> Handle(MakeStateTransitionCommand request, CancellationToken cancellationToken)
        {
            var processInstance = await _eventStoreRepository.GetLastAggregate<ProcessInstanceAggregate>(request.FlowNodeInstanceId, ProcessInstanceAggregate.GetStreamName(request.FlowNodeInstanceId));
            if (processInstance == null)
            {
                _logger.LogError($"unknown process instance '{request.FlowNodeInstanceId}'");
                throw new UnknownFlowInstanceException(string.Format(Global.UnknownProcessInstance, request.FlowNodeInstanceId));
            }

            if (!processInstance.ElementInstances.Any(_ => _.Id == request.FlowNodeElementInstanceId))
            {
                _logger.LogError($"unknown process element instance '{request.FlowNodeElementInstanceId}'");
                throw new UnknownFlowNodeElementInstanceException(string.Format(Global.UnknownProcessElementInstance, request.FlowNodeElementInstanceId));
            }

            await _messageBroker.QueueStateTransition(request.FlowNodeInstanceId, request.FlowNodeElementInstanceId, request.State, request.Parameters, cancellationToken);
            return true;
        }
    }
}
