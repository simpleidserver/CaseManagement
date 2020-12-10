using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.EventHandlers
{
    public class CasePlanEventHandler : IDomainEvtConsumerGeneric<CaseFilePublishedEvent>,
        IDomainEvtConsumerGeneric<CasePlanInstanceCreatedEvent>
    {
        private readonly ICaseFileQueryRepository _caseFileQueryRepository;
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;
        private readonly ICasePlanQueryRepository _casePlanQueryRepository;

        public CasePlanEventHandler(
            ICaseFileQueryRepository caseFileQueryRepository, 
            ICasePlanCommandRepository casePlanCommandRepository,
            ICasePlanQueryRepository casePlanQueryRepository)
        {
            _caseFileQueryRepository = caseFileQueryRepository;
            _casePlanCommandRepository = casePlanCommandRepository;
            _casePlanQueryRepository = casePlanQueryRepository;
        }

        public async Task Handle(CaseFilePublishedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileQueryRepository.Get(@event.AggregateId, cancellationToken);
            var tDefinitions = CMMNParser.ParseWSDL(caseFile.Payload);
            foreach (var casePlan in CMMNParser.ExtractCasePlans(tDefinitions, caseFile))
            {
                await _casePlanCommandRepository.Add(casePlan, cancellationToken);
            }

            await _casePlanCommandRepository.SaveChanges(cancellationToken);
        }

        public async Task Handle(CasePlanInstanceCreatedEvent message, CancellationToken token)
        {
            var casePlan = await _casePlanQueryRepository.Get(message.CasePlanId, token);
            casePlan.IncrementInstance();
            await _casePlanCommandRepository.Update(casePlan, token);
            await _casePlanCommandRepository.SaveChanges(token);
        }
    }
}