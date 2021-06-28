using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using MassTransit;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.EventHandlers
{
    public class CasePublishedEventConsumer : IConsumer<CaseFilePublishedEvent>
    {
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;

        public CasePublishedEventConsumer(
            ICaseFileCommandRepository caseFileCommandRepository,
            ICasePlanCommandRepository casePlanCommandRepository)
        {
            _caseFileCommandRepository = caseFileCommandRepository;
            _casePlanCommandRepository = casePlanCommandRepository;
        }

        public async Task Consume(ConsumeContext<CaseFilePublishedEvent> context)
        {
            var caseFile = await _caseFileCommandRepository.Get(context.Message.AggregateId, CancellationToken.None);
            var tDefinitions = CMMNParser.ParseWSDL(caseFile.Payload);
            foreach (var casePlan in CMMNParser.ExtractCasePlans(tDefinitions, caseFile))
            {
                await _casePlanCommandRepository.Add(casePlan, CancellationToken.None);
            }

            await _casePlanCommandRepository.SaveChanges(CancellationToken.None);
        }
    }
}
