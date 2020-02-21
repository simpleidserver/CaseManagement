using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.EventHandlers
{
    public class CasePlanEventHandler : IMessageBrokerConsumerGeneric<CaseFilePublishedEvent>
    {
        private readonly ICaseFileQueryRepository _caseFileQueryRepository;
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public CasePlanEventHandler(ICaseFileQueryRepository caseFileQueryRepository, ICasePlanCommandRepository casePlanCommandRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _caseFileQueryRepository = caseFileQueryRepository;
            _casePlanCommandRepository = casePlanCommandRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public string QueueName => CMMNConstants.QueueNames.CaseFiles;

        public async Task Handle(CaseFilePublishedEvent @event, CancellationToken cancellationToken)
        {
            var caseFile = await _caseFileQueryRepository.FindById(@event.AggregateId);
            var tDefinitions = CMMNParser.ParseWSDL(caseFile.Payload);
            foreach (var casePlan in CMMNParser.ExtractCasePlans(tDefinitions, caseFile))
            {
                _casePlanCommandRepository.Add(casePlan);
                await _commitAggregateHelper.Commit(casePlan, CasePlanAggregate.GetStreamName(casePlan.Id), CMMNConstants.QueueNames.CasePlans);
            }

            await _casePlanCommandRepository.SaveChanges();
        }
    }
}
