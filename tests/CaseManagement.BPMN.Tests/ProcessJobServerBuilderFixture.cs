using CaseManagement.BPMN.Builders;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaseManagement.BPMN.Tests
{
    public class ProcessJobServerBuilderFixture
    {
        [Fact]
        public async Task When_Execute_Sequence_Of_Empty_Tasks()
        {
            var id = ProcessInstanceAggregate.BuildId("processFile", "processId");
            var processInstance = ProcessInstanceBuilder.New("processId", "processFile")
                .AddStartEvent("1", "evt")
                .AddEmptyTask("2", "name", _ =>
                {
                    _.AddIncoming("1");
                })
                .AddEmptyTask("3", "name", _ =>
                {
                    _.AddIncoming("1");
                })
                .AddEmptyTask("4", "name", _ =>
                {
                    _.AddIncoming("2");
                    _.AddIncoming("3");
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            try
            {
                await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueProcessInstance(id, CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.Elements.All(_ => _.LastTransition == BPMNTransitions.COMPLETE);
                }, CancellationToken.None);
            }
            finally
            {
                jobServer.Stop();
            }
            string sss = "";
        }

        private class FakeCaseJobServer
        {
            private IServiceProvider _serviceProvider;
            private IProcessJobServer _processJobServer;
            private IProcessInstanceQueryRepository _processInstanceQueryRepository;

            private FakeCaseJobServer()
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddProcessJobServer();
                _serviceProvider = serviceCollection.BuildServiceProvider();
                _processJobServer = _serviceProvider.GetRequiredService<IProcessJobServer>();
                _processInstanceQueryRepository = _serviceProvider.GetRequiredService<IProcessInstanceQueryRepository>();
            }

            public static FakeCaseJobServer New()
            {
                var result = new FakeCaseJobServer();
                return result;
            }

            public void Start()
            {
                _processJobServer.Start();
            }

            public void Stop()
            {
                _processJobServer.Stop();
            }

            public Task RegisterProcessInstance(ProcessInstanceAggregate processInstance, CancellationToken token)
            {
                return _processJobServer.RegisterProcessInstance(processInstance, token);
            }

            public Task EnqueueProcessInstance(string processInstanceId, CancellationToken token)
            {
                return _processJobServer.EnqueueProcessInstance(processInstanceId, token);
            }

            public async Task<ProcessInstanceAggregate> MonitorProcessInstance(string id, Func<ProcessInstanceAggregate, bool> callback, CancellationToken token)
            {
                while (true)
                {
                    Thread.Sleep(100);
                    var result = await _processInstanceQueryRepository.Get(id, token);
                    if (callback(result))
                    {
                        return result;
                    }
                }
            }
        }
    }
}
