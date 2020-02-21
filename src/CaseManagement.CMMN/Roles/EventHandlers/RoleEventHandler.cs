using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles.EventHandlers
{
    public class RoleEventHandler : IMessageBrokerConsumerGeneric<CasePlanAddedEvent>
    {
        private readonly ICasePlanQueryRepository _casePlanQueryRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly IRoleCommandRepository _roleCommandRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public RoleEventHandler(ICasePlanQueryRepository casePlanQueryRepository, IRoleQueryRepository roleQueryRepository, IRoleCommandRepository roleCommandRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _casePlanQueryRepository = casePlanQueryRepository;
            _roleQueryRepository = roleQueryRepository;
            _roleCommandRepository = roleCommandRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlans;

        public async Task Handle(CasePlanAddedEvent message, CancellationToken token)
        {
            var casePlan = await _casePlanQueryRepository.FindById(message.AggregateId);
            var roles = await _roleQueryRepository.FindRoles(casePlan.Roles);
            var unknownRoles = casePlan.Roles.Where(r => !roles.Any(ro => ro.Id == r));
            foreach(var unknownRole in unknownRoles)
            {
                var role = RoleAggregate.New(unknownRole);
                _roleCommandRepository.Add(role);
                await _commitAggregateHelper.Commit(role, RoleAggregate.GetStreamName(unknownRole), CMMNConstants.QueueNames.Roles);
            }

            await _roleCommandRepository.SaveChanges();
        }
    }
}