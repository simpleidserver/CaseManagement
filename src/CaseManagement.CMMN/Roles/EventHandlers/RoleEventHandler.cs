using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Role.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles.EventHandlers
{
    public class RoleEventHandler : IMessageBrokerConsumerGeneric<RoleUpdatedEvent>, IMessageBrokerConsumerGeneric<RoleDeletedEvent>, IMessageBrokerConsumerGeneric<RoleAddedEvent>
    {
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly IRoleCommandRepository _roleCommandRepository;
        private readonly IDistributedLock _distributedLock;

        public RoleEventHandler(IRoleQueryRepository roleQueryRepository, IRoleCommandRepository roleCommandRepository, ICommitAggregateHelper commitAggregateHelper, IDistributedLock distributedLock)
        {
            _roleQueryRepository = roleQueryRepository;
            _roleCommandRepository = roleCommandRepository;
            _distributedLock = distributedLock;
        }

        public string QueueName => CMMNConstants.QueueNames.Roles;

        public async Task Handle(RoleUpdatedEvent message, CancellationToken token)
        {
            var lockId = $"update-role-{message.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            var role = await _roleQueryRepository.FindById(message.AggregateId);
            role.Handle(message);
            _roleCommandRepository.Update(role);
            await _roleCommandRepository.SaveChanges();
            await _distributedLock.ReleaseLock(lockId);
        }

        public async Task Handle(RoleDeletedEvent message, CancellationToken token)
        {
            var lockId = $"delete-role-{message.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            var role = await _roleQueryRepository.FindById(message.AggregateId);
            role.Handle(message);
            _roleCommandRepository.Update(role);
            await _roleCommandRepository.SaveChanges();
            await _distributedLock.ReleaseLock(lockId);
        }

        public async Task Handle(RoleAddedEvent message, CancellationToken token)
        {
            var lockId = $"add-role-{message.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                return;
            }

            var role = RoleAggregate.New(new[] { message });
            _roleCommandRepository.Add(role);
            await _roleCommandRepository.SaveChanges();
            await _distributedLock.ReleaseLock(lockId);
        }
    }
}
