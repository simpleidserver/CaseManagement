using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CaseInstance.Events;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CMMNProcessInstanceCreatedEventHandler : IDomainEventHandler<CMMNProcessInstanceCreatedEvent>
    {
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;

        public CMMNProcessInstanceCreatedEventHandler(IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository)
        {
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
        }

        public async Task Handle(CMMNProcessInstanceCreatedEvent @event, CancellationToken token)
        {
            var processFlowInstance = CMMNProcessFlowInstance.NewCMMNProcess(new List<DomainEvent> { @event });
            _processFlowInstanceCommandRepository.Add(processFlowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
