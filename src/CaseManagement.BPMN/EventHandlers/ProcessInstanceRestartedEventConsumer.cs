using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.ProcessInstance.Commands;
using MassTransit;
using MediatR;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.EventHandlers
{
    public class ProcessInstanceRestartedEventConsumer : IConsumer<ProcessInstanceRestartedEvent>
    {
        private readonly IMediator _mediator;

        public ProcessInstanceRestartedEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Consume(ConsumeContext<ProcessInstanceRestartedEvent> context)
        {
            return _mediator.Send(new StartProcessInstanceCommand 
            { 
                ProcessInstanceId = context.Message.AggregateId,
                NewExecutionPath = false
            });
        }
    }
}
