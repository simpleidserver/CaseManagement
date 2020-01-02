using CaseManagement.CMMN.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CMMNWorkflowElementInstanceFormCreatedEventHandler : IDomainEventHandler<CMMNWorkflowElementInstanceFormCreatedEvent>
    {
        public Task Handle(CMMNWorkflowElementInstanceFormCreatedEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
