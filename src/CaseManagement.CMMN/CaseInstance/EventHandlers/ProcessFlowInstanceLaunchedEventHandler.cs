using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class ProcessFlowInstanceLaunchedEventHandler : IDomainEventHandler<ProcessFlowInstanceLaunchedEvent>
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;

        public ProcessFlowInstanceLaunchedEventHandler(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
        }

        public async Task Handle(ProcessFlowInstanceLaunchedEvent @event, CancellationToken token)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Launch();
            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
