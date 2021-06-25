using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Processors;
using CaseManagement.BPMN.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Commands.Handlers
{
    public class MakeStateTransitionCommandHandler : IRequestHandler<MakeStateTransitionCommand, bool>
    {
        private readonly IProcessInstanceCommandRepository _processInstanceCommandRepository;
        private readonly IProcessInstanceProcessor _processInstanceProcessor;
        private readonly ILogger<MakeStateTransitionCommandHandler> _logger;

        public MakeStateTransitionCommandHandler(
            IProcessInstanceCommandRepository processInstanceCommandRepository,
            IProcessInstanceProcessor processInstanceProcessor,
            ILogger<MakeStateTransitionCommandHandler> logger)
        {
            _processInstanceCommandRepository = processInstanceCommandRepository;
            _processInstanceProcessor = processInstanceProcessor;
            _logger = logger;
        }

        public async Task<bool> Handle(MakeStateTransitionCommand request, CancellationToken cancellationToken)
        {
            var processInstance = await _processInstanceCommandRepository.Get(request.FlowNodeInstanceId, cancellationToken);
            if (processInstance == null)
            {
                _logger.LogError($"unknown process instance '{request.FlowNodeInstanceId}'");
                throw new UnknownFlowInstanceException(string.Format(Global.UnknownProcessInstance, request.FlowNodeInstanceId));
            }

            if (!processInstance.ElementInstances.Any(_ => _.EltId == request.FlowNodeElementInstanceId))
            {
                _logger.LogError($"unknown process element instance '{request.FlowNodeElementInstanceId}'");
                throw new UnknownFlowNodeElementInstanceException(string.Format(Global.UnknownProcessElementInstance, request.FlowNodeElementInstanceId));
            }

            var content = request.Parameters == null ? string.Empty : request.Parameters.ToString();
            processInstance.ConsumeStateTransition(request.FlowNodeElementInstanceId, request.State, content);
            await _processInstanceProcessor.Execute(processInstance, cancellationToken);
            await _processInstanceCommandRepository.Update(processInstance, cancellationToken);
            await _processInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
