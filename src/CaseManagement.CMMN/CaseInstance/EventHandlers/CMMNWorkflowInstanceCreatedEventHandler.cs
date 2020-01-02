using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CMMNWorkflowInstanceCreatedEventHandler : IDomainEventHandler<CMMNWorkflowInstanceCreatedEvent>
    {
        private readonly ICMMNWorkflowInstanceCommandRepository _cmmnWorkflowInstanceCommandRepository;

        public CMMNWorkflowInstanceCreatedEventHandler(ICMMNWorkflowInstanceCommandRepository cmmnWorkflowInstanceCommandRepository)
        {
            _cmmnWorkflowInstanceCommandRepository = cmmnWorkflowInstanceCommandRepository;
        }

        public async Task Handle(CMMNWorkflowInstanceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var processFlowInstance = CMMNWorkflowInstance.New(new List<DomainEvent>
            {
                @event
            });
            _cmmnWorkflowInstanceCommandRepository.Add(processFlowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }
    }
}
