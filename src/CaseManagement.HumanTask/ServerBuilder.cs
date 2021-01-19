using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.HumanTask.Authorization;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CaseManagement.HumanTask
{
    public class ServerBuilder
    {
        private IServiceCollection _services;

        public ServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ServerBuilder AddHumanTaskDefs(ICollection<HumanTaskDefinitionAggregate> humanTaskDefs)
        {
            var lst = new ConcurrentBag<HumanTaskDefinitionAggregate>(humanTaskDefs);
            _services.TryUpdateSingleton<IHumanTaskDefCommandRepository>(new HumanTaskDefCommandRepository(lst));
            _services.TryUpdateSingleton<IHumanTaskDefQueryRepository>(new HumanTaskDefQueryRepository(lst));
            return this;
        }

        public ServerBuilder AddNotificationDefs(ICollection<NotificationDefinitionAggregate> notificationDefs)
        {
            var lst = new ConcurrentBag<NotificationDefinitionAggregate>(notificationDefs);
            _services.TryUpdateSingleton<INotificationDefCommandRepository>(new NotificationDefCommandRepository(lst));
            _services.TryUpdateSingleton<INotificationDefQueryRepository>(new NotificationDefQueryRepository(lst));
            return this;
        }

        public ServerBuilder AddLogicalPeopleGroups(ICollection<LogicalPeopleGroup> logicalPeopleGroups)
        {
            var lst = new ConcurrentBag<LogicalPeopleGroup>(logicalPeopleGroups);
            _services.TryUpdateSingleton<ILogicalPeopleGroupStore>(new InMemoryLogicalPeopleGroupStore(lst));
            return this;
        }

        public ServerBuilder AddScheduledJobs(List<ScheduleJob> jobs)
        {
            var lst = new ConcurrentBag<ScheduleJob>(jobs);
            _services.TryUpdateSingleton<IScheduledJobStore>(new InMemoryScheduledJobStore(lst));
            return this;
        }
    }
}