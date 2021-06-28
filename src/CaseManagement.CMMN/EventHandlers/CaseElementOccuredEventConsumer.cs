using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.Domains;
using MassTransit;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.EventHandlers
{
    public class CaseElementOccuredEventConsumer : IConsumer<CaseElementOccuredEvent>
    {
        private readonly IMediator _mediator;

        public CaseElementOccuredEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CaseElementOccuredEvent> context)
        {
            await _mediator.Send(new OccurCommand(context.Message.AggregateId, context.Message.EltId)
            {
                Parameters = new Dictionary<string, string>()
            });
        }
    }
}
