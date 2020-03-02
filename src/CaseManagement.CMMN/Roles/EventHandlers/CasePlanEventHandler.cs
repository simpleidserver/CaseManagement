using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles.EventHandlers
{
    public class CasePlanEventHandler : IMessageBrokerConsumerGeneric<CasePlanAddedEvent>
    {
        private readonly ICasePlanQueryRepository _casePlanQueryRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IDistributedLock _distributedLock;

        public CasePlanEventHandler(ICasePlanQueryRepository casePlanQueryRepository, IRoleQueryRepository roleQueryRepository, IRoleCommandRepository roleCommandRepository, ICommitAggregateHelper commitAggregateHelper, IDistributedLock distributedLock)
        {
            _casePlanQueryRepository = casePlanQueryRepository;
            _roleQueryRepository = roleQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
            _distributedLock = distributedLock;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlans;

        public async Task Handle(CasePlanAddedEvent message, CancellationToken token)
        {
            var lockId = $"case-plan-added-{message.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            var casePlan = await _casePlanQueryRepository.FindById(message.AggregateId);
            var roles = await _roleQueryRepository.FindRoles(casePlan.Roles);
            var unknownRoles = casePlan.Roles.Where(r => !roles.Any(ro => ro.Id == r));
            foreach(var unknownRole in unknownRoles)
            {
                var role = RoleAggregate.New(unknownRole);
                await _commitAggregateHelper.Commit(role, RoleAggregate.GetStreamName(unknownRole), CMMNConstants.QueueNames.Roles);
            }

            await _distributedLock.ReleaseLock(lockId);
        }
    }
}