using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using MassTransit;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.EventHandlers
{
    public class CasePlanInstanceCreatedEventConsumer : IConsumer<CasePlanInstanceCreatedEvent>
    {
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;

        public CasePlanInstanceCreatedEventConsumer(ICasePlanCommandRepository casePlanCommandRepository)
        {
            _casePlanCommandRepository = casePlanCommandRepository;
        }

        public async Task Consume(ConsumeContext<CasePlanInstanceCreatedEvent> context)
        {
            var casePlan = await _casePlanCommandRepository.Get(context.Message.CasePlanId, CancellationToken.None);
            casePlan.IncrementInstance();
            await _casePlanCommandRepository.Update(casePlan, CancellationToken.None);
            await _casePlanCommandRepository.SaveChanges(CancellationToken.None);
        }
    }
}