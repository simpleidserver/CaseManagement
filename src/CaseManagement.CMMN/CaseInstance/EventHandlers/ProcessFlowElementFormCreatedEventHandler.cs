using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class ProcessFlowElementFormCreatedEventHandler : IDomainEventHandler<ProcessFlowElementFormCreatedEvent>
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;
        private readonly IFormQueryRepository _formQueryRepository;
        private readonly IFormInstanceCommandRepository _formInstanceCommandRepository;

        public ProcessFlowElementFormCreatedEventHandler(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository, IFormQueryRepository formQueryRepository, IFormInstanceCommandRepository formInstanceCommandRepository)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
            _formQueryRepository = formQueryRepository;
            _formInstanceCommandRepository = formInstanceCommandRepository;
        }

        public async Task Handle(ProcessFlowElementFormCreatedEvent @event, CancellationToken token)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var form = await _formQueryRepository.FindFormById(@event.FormId);
            var formInstance = FormInstanceAggregate.New(@event.FormInstanceId, @event.FormId);
            formInstance.Titles = form.Titles.Select(t => (Translation)t.Clone()).ToList();
            formInstance.UpdateDateTime = DateTime.UtcNow;
            formInstance.CreateDateTime = DateTime.UtcNow;
            formInstance.RoleId = @event.PerformerRef;
            foreach(var elt in form.Elements)
            {
                formInstance.AddElement(new FormInstanceElement
                {
                    FormElementId = elt.Id,
                    Value = string.Empty,
                    Descriptions = elt.Descriptions.Select(d => (Translation)d.Clone()).ToList(),
                    IsRequired = elt.IsRequired,
                    Names = elt.Names.Select(n => (Translation)n.Clone()).ToList(),
                    Type = elt.Type
                });
            }

            _formInstanceCommandRepository.Add(formInstance);
            flowInstance.Handle(@event);
            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _formInstanceCommandRepository.SaveChanges();
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
