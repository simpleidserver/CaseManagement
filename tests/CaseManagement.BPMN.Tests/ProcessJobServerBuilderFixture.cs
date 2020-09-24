using CaseManagement.BPMN.Builders;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Tests.Delegates;
using CaseManagement.BPMN.Tests.ItemDefs;
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
                .AddEmptyTask("2", "name")
                .AddEmptyTask("3", "name")
                .AddEmptyTask("4", "name")
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .AddSequenceFlow("seq2", "sequence", "1", "3")
                .AddSequenceFlow("seq3", "sequence", "2", "4")
                .AddSequenceFlow("seq3", "sequence", "3", "4")
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
                .AddMessage(messageName, "message", string.Empty)
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddEmptyTask("2", "name", _ =>
                {
                    _.SetStartQuantity(2);
                })
                .AddSequenceFlow("seq1", "sequence", "1", "2")
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
                await jobServer.EnqueueMessage(id, messageName, null, CancellationToken.None);
                casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.Count() == 2;
                }, CancellationToken.None);
                await jobServer.EnqueueMessage(id, messageName, null, CancellationToken.None);
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

        [Fact]
        public async Task When_Execute_ServiceTask_With_CSHARPCallback()
        {
            const string messageName = "message";
            var id = ProcessInstanceAggregate.BuildId("1", "processId", "processFile");
            var processInstance = ProcessInstanceBuilder.New("1", "processId", "processFile")
                .AddMessage(messageName, "message", string.Empty)
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddServiceTask("2", "name", _ =>
                {
                    _.SetImplementation(BPMNConstants.ImplementationNames.CALLBACK);
                    _.SetClassName(typeof(CreatePersonDelegate).FullName);
                })
                .AddSequenceFlow("seq1", "sequence", "1", "2")
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
                await jobServer.EnqueueMessage(id, messageName, null, CancellationToken.None);
                casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.Count() == 2;
                }, CancellationToken.None);
                var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
                var serviceTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
                Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
                Assert.Equal(ActivityStates.COMPLETED, serviceTaskInstance.ActivityState);
                Assert.Equal(FlowNodeStates.Complete, serviceTaskInstance.State);
            }
            finally
            {
                jobServer.Stop();
            }
        }

        [Fact]
        public async Task When_Execute_ConditionalSequenceFlow()
        {
            const string messageName = "message";
            var id = ProcessInstanceAggregate.BuildId("1", "processId", "processFile");
            var processInstance = ProcessInstanceBuilder.New("1", "processId", "processFile")
                .AddMessage(messageName, "message", "item")
                .AddItemDef("item", ItemKinds.Information, false, typeof(PersonParameter).FullName)
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddEmptyTask("2", "name")
                .AddEmptyTask("3", "name")
                .AddSequenceFlow("seq1", "sequence", "1", "2", "context.GetIncomingMessage(\"Firstname\") == \"user\"")
                .AddSequenceFlow("seq2", "sequence", "1", "3", "context.GetIncomingMessage(\"Firstname\") == \"baduser\"")
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
                await jobServer.EnqueueMessage(id, messageName, new PersonParameter { Firstname = "user" }, CancellationToken.None);
                casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.Count() == 2;
                }, CancellationToken.None);
                var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
                var firstEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
                Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
                Assert.Equal(ActivityStates.COMPLETED, firstEmptyTaskInstance.ActivityState);
                Assert.Equal(FlowNodeStates.Complete, firstEmptyTaskInstance.State);
            }
            finally
            {
                jobServer.Stop();
            }
        }

        [Fact]
        public async Task When_Execute_ExclusiveGateway()
        {
            const string messageName = "message";
            var id = ProcessInstanceAggregate.BuildId("1", "processId", "processFile");
            var processInstance = ProcessInstanceBuilder.New("1", "processId", "processFile")
                .AddMessage(messageName, "message", "item")
                .AddItemDef("item", ItemKinds.Information, false, typeof(PersonParameter).FullName)
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddExclusiveGateway("2", "gateway", GatewayDirections.DIVERGING)
                .AddEmptyTask("3", "name")
                .AddEmptyTask("4", "name")
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .AddSequenceFlow("seq2", "sequence", "2", "3", "context.GetIncomingMessage(\"Firstname\") == \"user\"")
                .AddSequenceFlow("seq3", "sequence", "2", "4", "context.GetIncomingMessage(\"Firstname\") == \"baduser\"")
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
                await jobServer.EnqueueMessage(id, messageName, new PersonParameter { Firstname = "user" }, CancellationToken.None);
                casePlanInstance = await jobServer.MonitorProcessInstance(id, (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.ElementInstances.Count() == 3;
                }, CancellationToken.None);
                var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
                var exclusiveGatewayInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
                var firstEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "3");
                Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
                Assert.Equal(FlowNodeStates.Complete, exclusiveGatewayInstance.State);
                Assert.Equal(ActivityStates.COMPLETED, firstEmptyTaskInstance.ActivityState);
                Assert.Equal(FlowNodeStates.Complete, firstEmptyTaskInstance.State);
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

            public Task EnqueueMessage(string processInstanceId, string messageName, object content, CancellationToken token)
            {
                return _processJobServer.EnqueueMessage(processInstanceId, messageName, content, token);
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
