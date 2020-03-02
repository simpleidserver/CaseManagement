using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan.EventHandlers
{
    public class CasePlanEventHandler : IMessageBrokerConsumerGeneric<CaseFilePublishedEvent>
    {
        private readonly ICaseFileQueryRepository _caseFileQueryRepository;
        private readonly ICasePlanCommandRepository _casePlanCommandRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IDistributedLock _distributedLock;

        public CasePlanEventHandler(ICaseFileQueryRepository caseFileQueryRepository, ICasePlanCommandRepository casePlanCommandRepository, ICommitAggregateHelper commitAggregateHelper, IDistributedLock distributedLock)
        {
            _caseFileQueryRepository = caseFileQueryRepository;
            _casePlanCommandRepository = casePlanCommandRepository;
            _commitAggregateHelper = commitAggregateHelper;
            _distributedLock = distributedLock;
        }

        public string QueueName => CMMNConstants.QueueNames.CaseFiles;

        public async Task Handle(CaseFilePublishedEvent @event, CancellationToken cancellationToken)
        {
            var lockId = $"case-file-published-{@event.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            var caseFile = await _caseFileQueryRepository.FindById(@event.AggregateId);
            var tDefinitions = CMMNParser.ParseWSDL(caseFile.Payload);
            foreach (var casePlan in CMMNParser.ExtractCasePlans(tDefinitions, caseFile))
            {
                _casePlanCommandRepository.Add(casePlan);
                await _commitAggregateHelper.Commit(casePlan, CasePlanAggregate.GetStreamName(casePlan.Id), CMMNConstants.QueueNames.CasePlans);
            }

            await _casePlanCommandRepository.SaveChanges();
            await _distributedLock.ReleaseLock(lockId);
        }
    }
}