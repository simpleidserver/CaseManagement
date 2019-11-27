using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Persistence;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class ProcessFlowInstanceLaunchedEventHandler : IDomainEventHandler<ProcessFlowInstanceLaunchedEvent>
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;
        private readonly IWorkflowEngine _workflowEngine;

        public ProcessFlowInstanceLaunchedEventHandler(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository, IWorkflowEngine workflowEngine)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
            _workflowEngine = workflowEngine;
        }

        public async Task Handle(ProcessFlowInstanceLaunchedEvent @event)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(@event.Id);
            flowInstance.Launch();
            var context = new ProcessFlowInstanceExecutionContext(flowInstance);
            await _workflowEngine.Start(flowInstance, context);
            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
