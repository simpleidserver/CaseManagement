using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class ProcessFlowElementFormConfirmedEventHandler : IDomainEventHandler<ProcessFlowElementFormConfirmedEvent>
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;
        private readonly IFormInstanceQueryRepository _formInstanceQueryRepository;
        private readonly IFormInstanceCommandRepository _formInstanceCommandRepository;

        public ProcessFlowElementFormConfirmedEventHandler(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository, IFormInstanceQueryRepository formInstanceQueryRepository, IFormInstanceCommandRepository formInstanceCommandRepository)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
            _formInstanceQueryRepository = formInstanceQueryRepository;
            _formInstanceCommandRepository = formInstanceCommandRepository;
        }

        public async Task Handle(ProcessFlowElementFormConfirmedEvent @event, CancellationToken token)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var formInstance = await _formInstanceQueryRepository.FindById(@event.FormInstanceId);
            foreach(var kvp in @event.FormContent)
            {
                formInstance.UpdateElement(kvp.Key, kvp.Value);
            }

            formInstance.UpdateDateTime = DateTime.UtcNow;
            formInstance.Status = FormInstanceStatus.Complete;
            _formInstanceCommandRepository.Update(formInstance);
            flowInstance.Handle(@event);
            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _formInstanceCommandRepository.SaveChanges();
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
