using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class ProcessFlowInstanceCreatedEventHandler : IDomainEventHandler<ProcessFlowInstanceCreatedEvent>
    {
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;

        public ProcessFlowInstanceCreatedEventHandler(IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository)
        {
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
        }

        public async Task Handle(ProcessFlowInstanceCreatedEvent @event, CancellationToken token)
        {
            var processFlowInstance = ProcessFlowInstance.New(new List<DomainEvent> { @event });
            _processFlowInstanceCommandRepository.Add(processFlowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
