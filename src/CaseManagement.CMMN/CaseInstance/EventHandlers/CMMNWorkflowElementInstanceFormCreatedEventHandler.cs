using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CMMNWorkflowElementInstanceFormCreatedEventHandler : IDomainEventHandler<CMMNWorkflowElementInstanceFormCreatedEvent>
    {
        private readonly ICMMNWorkflowInstanceCommandRepository _cmmnWorkflowInstanceCommandRepository;
        private readonly ICMMNWorkflowInstanceQueryRepository _cmmnWorkflowInstanceQueryRepository;
        private readonly IFormInstanceCommandRepository _formInstanceCommandRepository;
        private readonly IFormQueryRepository _formQueryRepository;

        public CMMNWorkflowElementInstanceFormCreatedEventHandler(ICMMNWorkflowInstanceCommandRepository cmmnWorkflowInstanceCommandRepository, ICMMNWorkflowInstanceQueryRepository cmmnWorkflowInstanceQueryRepository, IFormInstanceCommandRepository formInstanceCommandRepository, IFormQueryRepository formQueryRepository)
        {
            _cmmnWorkflowInstanceCommandRepository = cmmnWorkflowInstanceCommandRepository;
            _cmmnWorkflowInstanceQueryRepository = cmmnWorkflowInstanceQueryRepository;
            _formInstanceCommandRepository = formInstanceCommandRepository;
            _formQueryRepository = formQueryRepository;
        }

        public async Task Handle(CMMNWorkflowElementInstanceFormCreatedEvent @event, CancellationToken cancellationToken)
        {
            // TODO : AJOUTER UN TEST CMMN POUR TESTER CASEFILEITEM.
            // TODO : IL FAUT AJOUTER L INSTANCE DE FORMULAIRE.
            // TODO : IL FAUT AUSSI AJOUTER LA POSSIBILITE DE RECUPERER LA LISTE DES ACTIVATIONS.
            var flowInstance = await _cmmnWorkflowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            flowInstance.Handle(@event);
            _cmmnWorkflowInstanceCommandRepository.Update(flowInstance);
            await _cmmnWorkflowInstanceCommandRepository.SaveChanges();
        }
    }
}
