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
        #region BPMN elements

        [Fact]
        public async Task When_Execute_Sequence_Of_Empty_Tasks()
        {
            var id = ProcessInstanceAggregate.BuildId("1", "processId", "processFile");
            var processInstance = ProcessInstanceBuilder.New("1", "processId", "processFile")
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
                await jobServer.EnqueueProcessInstance(id, true, CancellationToken.None);
                await jobServer.EnqueueProcessInstance(id, true, CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.Count() == 10;
                }, CancellationToken.None);
                Assert.Equal(10, casePlanInstance.ElementInstances.Count());
            }
            finally
            {
                jobServer.Stop();
            }
        }

        [Fact]
        public async Task When_Execute_StartEvent_With_MessageEventDefinition()
        {
            const string messageName = "alert";
            var id = ProcessInstanceAggregate.BuildId("1", "processId", "processFile");
            var processInstance = ProcessInstanceBuilder.New("1", "processId", "processFile")
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(new Message
                        {
                            Name = messageName
                        });
                    });
                })
                .AddEmptyTask("2", "name", _ =>
                {
                    _.AddIncoming("1");
                    _.SetStartQuantity(2);
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            try
            {
                await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueProcessInstance(id, true, CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.Any();
                }, CancellationToken.None);
                await jobServer.EnqueueMessage(id, messageName, CancellationToken.None);
                casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.Count() == 2;
                }, CancellationToken.None);
                await jobServer.EnqueueMessage(id, messageName, CancellationToken.None);
                casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.First(_ => _.FlowNodeId == "2").ActivityState == ActivityStates.COMPLETED;
                }, CancellationToken.None);
                var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
                var emptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
                Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
                Assert.Equal(ActivityStates.COMPLETED, emptyTaskInstance.ActivityState);
                Assert.Equal(FlowNodeStates.Complete, emptyTaskInstance.State);
                Assert.Equal(1, casePlanInstance.ExecutionPathLst.Count());
                Assert.Equal(1, casePlanInstance.ExecutionPathLst.First().ActivePointers.Count());
                Assert.Equal(4, casePlanInstance.ExecutionPathLst.First().Pointers.Count());
            }
            finally
            {
                jobServer.Stop();
            }
        }

        #endregion

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

            public Task EnqueueProcessInstance(string processInstanceId, bool isNewInstance, CancellationToken token)
            {
                return _processJobServer.EnqueueProcessInstance(processInstanceId, isNewInstance, token);
            }

            public Task EnqueueMessage(string processInstanceId, string messageName, CancellationToken token)
            {
                return _processJobServer.EnqueueMessage(processInstanceId, messageName, token);
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
