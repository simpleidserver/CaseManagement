using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures.DomainEvts;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.EventHandlers
{
    public class CasePlanEventHandler : IDomainEvtConsumerGeneric<CaseFilePublishedEvent>
    {
        private readonly ICaseFileQueryRepository _caseFileQueryRepository;
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;

        public CasePlanEventHandler(ICaseFileQueryRepository caseFileQueryRepository, ICasePlanCommandRepository casePlanCommandRepository)
        {
            _caseFileQueryRepository = caseFileQueryRepository;
            _casePlanCommandRepository = casePlanCommandRepository;
        }

        public async Task Handle(CaseFilePublishedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileQueryRepository.Get(@event.AggregateId, cancellationToken);
            var tDefinitions = CMMNParser.ParseWSDL(caseFile.Payload);
            foreach(var casePlan in CMMNParser.ExtractCasePlans(tDefinitions, caseFile))
            {
                _casePlanCommandRepository.Add(casePlan);
            }

            await _casePlanCommandRepository.SaveChanges(cancellationToken);
        }
    }
}