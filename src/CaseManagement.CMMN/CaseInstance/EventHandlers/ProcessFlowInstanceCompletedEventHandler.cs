using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class ProcessFlowInstanceCompletedEventHandler : IDomainEventHandler<ProcessFlowInstanceCompletedEvent>
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;

        public ProcessFlowInstanceCompletedEventHandler(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
        }

        public async Task Handle(ProcessFlowInstanceCompletedEvent @event, CancellationToken token)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
