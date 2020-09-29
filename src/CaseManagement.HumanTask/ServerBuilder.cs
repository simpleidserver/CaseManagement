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

        public ServerBuilder AddLogicalPeopleGroups(ICollection<LogicalPeopleGroup> logicalPeopleGroups)
        {
            var lst = new ConcurrentBag<LogicalPeopleGroup>(logicalPeopleGroups);
            _services.TryUpdateSingleton<ILogicalPeopleGroupStore>(new InMemoryLogicalPeopleGroupStore(lst));
            return this;
        }
    }
}