using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class FormInstanceHandler : IDomainEventHandler<CaseElementInstanceFormCreatedEvent>, IDomainEventHandler<CaseElementInstanceFormSubmittedEvent>
    {
        private readonly IFormInstanceCommandRepository _formInstanceCommandRepository;
        private readonly IFormInstanceQueryRepository _formInstanceQueryRepository;
        private readonly IFormQueryRepository _formQueryRepository;
        private readonly ICaseInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;

        public FormInstanceHandler(IFormInstanceCommandRepository formInstanceCommandRepository, IFormInstanceQueryRepository formInstanceQueryRepository, IFormQueryRepository formQueryRepository, ICaseInstanceQueryRepository cmmnWorkflowInstanceQueryRepository)
        {
            _formInstanceCommandRepository = formInstanceCommandRepository;
            _formInstanceQueryRepository = formInstanceQueryRepository;
            _formQueryRepository = formQueryRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
        }

        public async Task Handle(CaseElementInstanceFormCreatedEvent @event, CancellationToken cancellationToken)
        {
            var workflowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var form = await _formQueryRepository.FindFormById(@event.FormId);
            var formInstance = new FormInstanceAggregate(@event.FormInstanceId, @event.FormId)
            {
                CreateDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow,
                RoleId = @event.PerformerRef,
                Status = FormInstanceStatus.Create,
                CaseDefinitionId = workflowInstance.CaseDefinitionId,
                CaseInstanceId = workflowInstance.Id,
                CaseElementDefinitionId = workflowInstance.GetWorkflowElementInstance(@event.CaseElementId).CaseElementDefinitionId,
                CaseElementInstanceId = @event.CaseElementId,
                Titles = form.Titles,
                FormId = @event.FormId
            };
            foreach(var elt in form.Elements)
            {
                formInstance.AddElement(new FormInstanceElement
                {
                    Descriptions = elt.Descriptions,
                    FormElementId = elt.Id,
                    IsRequired = elt.IsRequired,
                    Names = elt.Names,
                    Type = elt.Type,
                    Value = string.Empty
                });
            }

            _formInstanceCommandRepository.Add(formInstance);
            await _formInstanceCommandRepository.SaveChanges();
        }

        public async Task Handle(CaseElementInstanceFormSubmittedEvent @event, CancellationToken cancellationToken)
        {
            var formInstance = await _formInstanceQueryRepository.FindById(@event.FormInstanceId);
            formInstance.UpdateDateTime = DateTime.UtcNow;
            formInstance.Status = FormInstanceStatus.Complete;
            foreach(var elt in formInstance.Content)
            {
                if (!@event.FormValues.ContainsKey(elt.FormElementId))
                {
                    continue;
                }

                elt.Value = @event.FormValues[elt.FormElementId];
            }

            _formInstanceCommandRepository.Update(formInstance);
            await _formInstanceCommandRepository.SaveChanges();
        }
    }
}
