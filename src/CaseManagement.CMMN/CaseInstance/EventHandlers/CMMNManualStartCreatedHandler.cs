using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CaseInstance.Events;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class CMMNManualStartCreatedHandler : IDomainEventHandler<CMMNManualStartCreated>
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;
        private readonly ICMMNActivationCommandRepository _cmmnActivationCommandRepository;

        public CMMNManualStartCreatedHandler(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository, ICMMNActivationCommandRepository cmmnActivationCommandRepository)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
            _cmmnActivationCommandRepository = cmmnActivationCommandRepository;
        }

        public async Task Handle(CMMNManualStartCreated @event, CancellationToken cancellationToken)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(@event.AggregateId);
            var elt = flowInstance.Elements.First(e => e.Id == @event.ElementId);
            var newActivation = new CaseActivationAggregate
            {
                Id = Guid.NewGuid().ToString(),
                CreateDateTime = DateTime.UtcNow,
                ElementId = elt.Id,
                ElementName = elt.Name,
                ProcessId = flowInstance.Id,
                ProcessName = flowInstance.ProcessFlowName
            };
            flowInstance.Handle(@event);
            _processFlowInstanceCommandRepository.Update(flowInstance);
            _cmmnActivationCommandRepository.Add(newActivation);
            await _processFlowInstanceCommandRepository.SaveChanges();
            await _cmmnActivationCommandRepository.SaveChanges();
        }
    }
}
