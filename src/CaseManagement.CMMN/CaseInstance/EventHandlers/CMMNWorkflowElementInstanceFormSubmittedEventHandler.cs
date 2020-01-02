using CaseManagement.CMMN.Domains.Events;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    class CMMNWorkflowElementInstanceFormSubmittedEventHandler : IDomainEventHandler<CMMNWorkflowElementInstanceFormSubmittedEvent>
    {
        public Task Handle(CMMNWorkflowElementInstanceFormSubmittedEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
