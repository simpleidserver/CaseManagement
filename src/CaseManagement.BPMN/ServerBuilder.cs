using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Persistence.InMemory;
using CaseManagement.Common;
using CaseManagement.Common.EvtStore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Threading;

namespace CaseManagement.BPMN
{
    public class ServerBuilder
    {
        private readonly IServiceCollection _services;

        public ServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ServerBuilder AddProcessInstances(ConcurrentBag<ProcessInstanceAggregate> processInstances)
        {
            _services.AddSingleton<IProcessInstanceQueryRepository>(new InMemoryProcessInstanceQueryRepository(processInstances));
            _services.AddSingleton<IProcessInstanceCommandRepository>(new InMemoryProcessInstanceCommandRepository(processInstances));
            var sp = _services.BuildServiceProvider();
            var commitAggregateHelper = sp.GetService<ICommitAggregateHelper>();
            foreach(var processInstance in processInstances)
            {
                commitAggregateHelper.Commit(processInstance, processInstance.GetStreamName(), CancellationToken.None).Wait();
            }

            return this;
        }
    }
}
