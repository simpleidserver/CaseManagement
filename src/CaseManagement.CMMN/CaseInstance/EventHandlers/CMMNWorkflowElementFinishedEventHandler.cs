using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CMMNWorkflowElementFinishedEventHandler : IDomainEventHandler<CMMNWorkflowElementFinishedEvent>
    {
        private readonly ICMMNWorkflowInstanceCommandRepository _cmmnWorkflowInstanceCommandRepository;
        private readonly ICMMNWorkflowInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public CMMNWorkflowElementFinishedEventHandler(ICMMNWorkflowInstanceCommandRepository cmmnWorkflowInstanceCommandRepository, ICMMNWorkflowInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _cmmnWorkflowInstanceCommandRepository = cmmnWorkflowInstanceCommandRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        public async Task Handle(CMMNWorkflowElementFinishedEvent @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }
    }
}
