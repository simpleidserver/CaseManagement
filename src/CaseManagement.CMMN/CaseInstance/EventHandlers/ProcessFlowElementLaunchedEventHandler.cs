using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Persistence;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.EventHandlers
{
    public class ProcessFlowElementLaunchedEventHandler : IDomainEventHandler<ProcessFlowElementLaunchedEvent>
    {
        private readonly IProcessFlowElementProcessorFactory _processFlowElementProcessorFactory;
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;

        public ProcessFlowElementLaunchedEventHandler(IProcessFlowElementProcessorFactory processFlowElementProcessorFactory, IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository)
        {
            _processFlowElementProcessorFactory = processFlowElementProcessorFactory;
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
        }

        public async Task Handle(ProcessFlowElementLaunchedEvent @event)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(@event.ProcessFlowInstanceId);
            await Start(flowInstance, flowInstance.Elements.First(e => e.Id == @event.ProcessFlowInstanceElementId));
            _processFlowInstanceCommandRepository.Update(flowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
        }

        private async Task Start(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceElement elt)
        {
            var processor = _processFlowElementProcessorFactory.Build(elt);
            await processor.Handle(processFlowInstance, elt);
            if (elt.Status != ProcessFlowInstanceElementStatus.Finished)
            {
                return;
            }
        
            var nextElts = processFlowInstance.NextElements(elt.Id);
            if (!nextElts.Any())
            {
                return;
            }
        
            foreach (var nextElt in nextElts)
            {
                await Start(processFlowInstance, nextElt);
            }
        }
    }
}
