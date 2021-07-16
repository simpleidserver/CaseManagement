using CaseManagement.BPMN.Builders;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Commands;
using CaseManagement.BPMN.Tests.Delegates;
using CaseManagement.BPMN.Tests.ItemDefs;
using CaseManagement.Common.Factories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var processInstance = ProcessInstanceBuilder.New("processFile")
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
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            Assert.Equal(10, casePlanInstance.ElementInstances.Count());
        }

        [Fact]
        public async Task When_Execute_StartEvent_With_MessageEventDefinition()
        {
            const string messageName = "message";
            var processInstance = ProcessInstanceBuilder.New("processFile")
                .AddMessage(messageName, "message", string.Empty)
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef("message");
                    });
                })
                .AddEmptyTask("2", "name", _ =>
                {
                    _.SetStartQuantity(2);
                })
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, null, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, null, CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
            var emptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
            Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
            Assert.Equal(ActivityStates.COMPLETED, emptyTaskInstance.ActivityState);
            Assert.Equal(FlowNodeStates.Complete, emptyTaskInstance.State);
            Assert.Equal(1, casePlanInstance.ExecutionPathLst.Count());
            Assert.Equal(1, casePlanInstance.ExecutionPathLst.First().ActivePointers.Count());
            Assert.Equal(4, casePlanInstance.ExecutionPathLst.First().Pointers.Count());
        }

        [Fact]
        public async Task When_Execute_ServiceTask_With_CSHARPCallback()
        {
            const string messageName = "message";
            var processInstance = ProcessInstanceBuilder.New("processFile")
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
                    _.SetDelegate("GetWeatherInformationDelegate");
                })
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, null, CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
            var serviceTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
            Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
            Assert.Equal(ActivityStates.COMPLETED, serviceTaskInstance.ActivityState);
            Assert.Equal(FlowNodeStates.Complete, serviceTaskInstance.State);
        }

        [Fact]
        public async Task When_Execute_ConditionalSequenceFlow()
        {
            const string messageName = "message";
            var processInstance = ProcessInstanceBuilder.New("processFile")
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
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, new PersonParameter { Firstname = "user" }, CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
            var firstEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
            Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
            Assert.Equal(ActivityStates.COMPLETED, firstEmptyTaskInstance.ActivityState);
            Assert.Equal(FlowNodeStates.Complete, firstEmptyTaskInstance.State);
        }

        [Fact]
        public async Task When_Execute_ExclusiveGateway()
        {
            const string messageName = "message";
            var processInstance = ProcessInstanceBuilder.New("processFile")
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
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, new PersonParameter { Firstname = "user" }, CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
            var exclusiveGatewayInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
            var firstEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "3");
            Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
            Assert.Equal(FlowNodeStates.Complete, exclusiveGatewayInstance.State);
            Assert.Equal(ActivityStates.COMPLETED, firstEmptyTaskInstance.ActivityState);
            Assert.Equal(FlowNodeStates.Complete, firstEmptyTaskInstance.State);
        }

        [Fact]
        public async Task When_Execute_ParallelGateway()
        {
            const string messageName = "message";
            var processInstance = ProcessInstanceBuilder.New("processFile")
                .AddMessage(messageName, "message", "item")
                .AddItemDef("item", ItemKinds.Information, false, typeof(PersonParameter).FullName)
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddParallelGateway("2", "gateway", GatewayDirections.DIVERGING)
                .AddEmptyTask("3", "name")
                .AddEmptyTask("4", "name")
                .AddParallelGateway("5", "gateway", GatewayDirections.CONVERGING)
                .AddEmptyTask("6", "name")
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .AddSequenceFlow("seq2", "sequence", "2", "3")
                .AddSequenceFlow("seq3", "sequence", "2", "4")
                .AddSequenceFlow("seq4", "sequence", "3", "5")
                .AddSequenceFlow("seq5", "sequence", "4", "5")
                .AddSequenceFlow("seq6", "sequence", "5", "6")
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, new PersonParameter { Firstname = "user" }, CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
            var firstParallelGateway = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
            var firstEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "3");
            var secondEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "4");
            var secondParallelGateway = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "5");
            var thirdEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "6");
            var secondParallelGatewayExecPointer = casePlanInstance.ExecutionPathLst.First().Pointers.First(_ => _.FlowNodeId == "5");
            var thirdEmptyTaskExecPointer = casePlanInstance.ExecutionPathLst.First().Pointers.First(_ => _.FlowNodeId == "6");
            Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
            Assert.Equal(FlowNodeStates.Complete, firstParallelGateway.State);
            Assert.Equal(FlowNodeStates.Complete, firstEmptyTaskInstance.State);
            Assert.Equal(FlowNodeStates.Complete, secondEmptyTaskInstance.State);
            Assert.Equal(FlowNodeStates.Complete, secondParallelGateway.State);
            Assert.Equal(FlowNodeStates.Complete, thirdEmptyTaskInstance.State);
            Assert.Equal(2, secondParallelGatewayExecPointer.Incoming.Count());
            Assert.Equal(2, thirdEmptyTaskExecPointer.Incoming.Count());
        }

        [Fact]
        public async Task When_Execute_InclusiveGateway()
        {
            const string messageName = "message";
            var processInstance = ProcessInstanceBuilder.New("1")
                .AddMessage(messageName, "message", "item")
                .AddItemDef("item", ItemKinds.Information, false, typeof(PersonParameter).FullName)
                .AddStartEvent("1", "evt", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddInclusiveGateway("2", "gateway", GatewayDirections.DIVERGING, "5")
                .AddEmptyTask("3", "name")
                .AddEmptyTask("4", "name")
                .AddEmptyTask("5", "name")
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .AddSequenceFlow("seq2", "sequence", "2", "3", "context.GetIncomingMessage(\"Firstname\") == \"user1\"")
                .AddSequenceFlow("seq3", "sequence", "2", "4", "context.GetIncomingMessage(\"Firstname\") == \"user2\"")
                .AddSequenceFlow("seq4", "sequence", "2", "5")
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, new PersonParameter { }, CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var startEventInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1");
            var inclusiveGatewayInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
            var firstEmptyTaskInstance = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "5");
            Assert.Equal(FlowNodeStates.Active, startEventInstance.State);
            Assert.Equal(FlowNodeStates.Complete, inclusiveGatewayInstance.State);
            Assert.Equal(FlowNodeStates.Complete, firstEmptyTaskInstance.State);
        }

        [Fact]
        public async Task When_Execute_UserTask()
        {
            var humanTaskInstanceId = Guid.NewGuid().ToString();
            var processInstance = ProcessInstanceBuilder.New("processFile")
                .AddStartEvent("1", "evt")
                .AddServiceTask("2", "serviceTask", (cb) =>
                {
                    cb.SetDelegate("GetWeatherInformationDelegate");
                })
                .AddUserTask("3", "userTask", (cb) =>
                {
                    cb.SetWsHumanTask("dressAppropriate", new Dictionary<string, string>
                    {
                        { "degree", "context.GetIncomingMessage(\"weatherInformation\", \"degree\")" },
                        { "city", "context.GetIncomingMessage(\"weatherInformation\", \"city\")" }
                    });
                })
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .AddSequenceFlow("seq2", "sequence", "2", "3")
                .Build();
            var jobServer = FakeCaseJobServer.New();
            jobServer.HttpMessageHandler.Protected()
                .As<IHttpMessageHandler>()
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{ 'id' : '" + humanTaskInstanceId + "', 'defId': 'defId' }")
                });
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var ei = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "3");
            await jobServer.EnqueueStateTransition(casePlanInstance.AggregateId, ei.EltId, "COMPLETED", new JObject(), CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            Assert.True(casePlanInstance.ElementInstances.All(_ => _.State == FlowNodeStates.Complete));
        }

        [Fact]
        public async Task When_Execute_BoundaryEvent()
        {
            var humanTaskInstanceId = Guid.NewGuid().ToString();
            const string messageName = "message";
            var processInstance = ProcessInstanceBuilder.New("processFile")
                .AddMessage(messageName, "message", "item")
                .AddMessage("humanTaskCreated", "humanTaskCreated", "item")
                .AddItemDef("item", ItemKinds.Information, false, typeof(PersonParameter).FullName)
                .AddStartEvent("1", "evt")
                .AddUserTask("2", "userTask", (cb) =>
                {
                    cb.SetWsHumanTask("dressAppropriate");
                    cb.AddBoundaryEventRef("3");
                })
                .AddBoundaryEvent("3", "messageReceived", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddEmptyTask("4", "emptyTask")
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .AddSequenceFlow("seq2", "sequence", "2", "3")
                .AddSequenceFlow("seq3", "sequence", "3", "4")
                .Build();
            var jobServer = FakeCaseJobServer.New();
            jobServer.HttpMessageHandler.Protected()
                .As<IHttpMessageHandler>()
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{ 'id' : '" + humanTaskInstanceId + "', 'defId': 'defId' }")
                });

            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            await jobServer.EnqueueMessage(processInstance.AggregateId, messageName, new PersonParameter { }, CancellationToken.None);
            var casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var ei = casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2");
            await jobServer.EnqueueStateTransition(casePlanInstance.AggregateId, ei.EltId, "COMPLETED", new JObject(), CancellationToken.None);
            casePlanInstance = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            var executionPath = casePlanInstance.ExecutionPathLst.First();
            Assert.True(casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "1").State == FlowNodeStates.Complete);
            Assert.True(casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "2").State == FlowNodeStates.Complete);
            Assert.True(casePlanInstance.ElementInstances.First(_ => _.FlowNodeId == "4").State == FlowNodeStates.Complete);
            Assert.Equal(1, executionPath.Pointers.First(p => p.FlowNodeId == "1").Outgoing.Count());
            Assert.Equal(1, executionPath.Pointers.First(p => p.FlowNodeId == "2").Incoming.Count());
            Assert.Equal(1, executionPath.Pointers.First(p => p.FlowNodeId == "2").Outgoing.Count());
            Assert.Equal(2, executionPath.Pointers.First(p => p.FlowNodeId == "3").Incoming.Count());
            Assert.Equal(2, executionPath.Pointers.First(p => p.FlowNodeId == "4").Incoming.Count());
        }

        [Fact]
        public async Task When_Execute_HumanTaskWithBoundaryEvent()
        {
            var humanTaskInstanceId = Guid.NewGuid().ToString();
            const string messageName = "humanTaskCreated";
            var processInstance = ProcessInstanceBuilder.New("processFile")
                .AddMessage(messageName, "humanTaskCreated", "item")
                .AddItemDef("item", ItemKinds.Information, false, typeof(HumanTaskParameter).FullName)
                .AddStartEvent("1", "evt")
                .AddUserTask("2", "userTask", (cb) =>
                {
                    cb.SetWsHumanTask("dressAppropriate");
                    cb.AddBoundaryEventRef("3");
                })
                .AddBoundaryEvent("3", "humanTaskCreated", _ =>
                {
                    _.AddMessageEvtDef("id", cb =>
                    {
                        cb.SetMessageRef(messageName);
                    });
                })
                .AddEmptyTask("4", "firstEmptyTask")
                .AddEmptyTask("5", "secondEmptyTask")
                .AddSequenceFlow("seq1", "sequence", "1", "2")
                .AddSequenceFlow("seq2", "sequence", "2", "3")
                .AddSequenceFlow("seq3", "sequence", "3", "4")
                .AddSequenceFlow("seq4", "sequence", "2", "5")
                .Build();
            var jobServer = FakeCaseJobServer.New();
            jobServer.Start();
            jobServer.HttpMessageHandler.Protected()
                .As<IHttpMessageHandler>()
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{ 'id' : '" + humanTaskInstanceId + "', 'defId': 'defId' }")
                });
            await jobServer.RegisterProcessInstance(processInstance, CancellationToken.None);
            await jobServer.EnqueueProcessInstance(processInstance.AggregateId, true, CancellationToken.None);
            var result = await jobServer.MonitoringProcessInstance(processInstance.AggregateId, (p) =>
            {
                var elt = p.ElementInstances.FirstOrDefault(_ => _.FlowNodeId == "4");
                return elt != null && elt.State == FlowNodeStates.Complete;
            }, CancellationToken.None);
            var ei = result.ElementInstances.First(_ => _.FlowNodeId == "2");
            await jobServer.EnqueueStateTransition(result.AggregateId, ei.EltId, "COMPLETED", new JObject(), CancellationToken.None);
            result = await jobServer.Get(processInstance.AggregateId, CancellationToken.None);
            Assert.True(result.ElementInstances.First(_ => _.FlowNodeId == "1").State == FlowNodeStates.Complete);
            Assert.True(result.ElementInstances.First(_ => _.FlowNodeId == "2").State == FlowNodeStates.Complete);
            Assert.True(result.ElementInstances.First(_ => _.FlowNodeId == "4").State == FlowNodeStates.Complete);
            jobServer.Stop();
        }

        #endregion

        private class FakeCaseJobServer
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly IProcessInstanceCommandRepository _processInstanceCommandRepository;
            private readonly IBusControl _busControl;
            private readonly IMediator _mediator;
            private FakeHttpClientFactory _factory;

            private FakeCaseJobServer()
            {
                _factory = new FakeHttpClientFactory();
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddLogging();
                serviceCollection.AddProcessJobServer(callbackServerOpts: o =>
                {
                    o.WSHumanTaskAPI = "http://localhost";
                    o.CallbackUrl = "http://localhost/{id}/{eltId}";
                }).AddDelegateConfigurations(new ConcurrentBag<DelegateConfigurationAggregate>
                {
                    DelegateConfigurationAggregate.Create("GetWeatherInformationDelegate", typeof(GetWeatherInformationDelegate).FullName)
                });
                serviceCollection.AddSingleton<IHttpClientFactory>(_factory);
                _serviceProvider = serviceCollection.BuildServiceProvider();
                _processInstanceCommandRepository = _serviceProvider.GetRequiredService<IProcessInstanceCommandRepository>();
                _busControl = _serviceProvider.GetRequiredService<IBusControl>();
                _mediator = _serviceProvider.GetRequiredService<IMediator>();
            }

            public Mock<HttpMessageHandler> HttpMessageHandler => _factory.MockHttpHandler;

            public void Start()
            {
                _busControl.Start();
            }

            public void Stop()
            {
                _busControl.Stop();
            }

            public static FakeCaseJobServer New()
            {
                var result = new FakeCaseJobServer();
                return result;
            }

            public async Task<ProcessInstanceAggregate> MonitoringProcessInstance(string id, Func<ProcessInstanceAggregate, bool> callback, CancellationToken token)
            {
                while (true)
                {
                    Thread.Sleep(100);
                    var result = await _processInstanceCommandRepository.Get(id, token);
                    if (callback(result))
                    {
                        return result;
                    }
                }
            }

            public Task<ProcessInstanceAggregate> Get(string id, CancellationToken token)
            {
                return _processInstanceCommandRepository.Get(id, token);
            }

            public async Task RegisterProcessInstance(ProcessInstanceAggregate processInstance, CancellationToken token)
            {
                await _processInstanceCommandRepository.Add(processInstance, token);
            }

            public Task EnqueueProcessInstance(string processInstanceId, bool isNewInstance, CancellationToken token)
            {
                return _mediator.Send(new StartProcessInstanceCommand
                {
                    NewExecutionPath = isNewInstance,
                    ProcessInstanceId = processInstanceId
                }, token);
            }

            public Task EnqueueStateTransition(string processInstanceId, string flowNodeInstanceId, string state, JObject jObj, CancellationToken token)
            {
                return _mediator.Send(new MakeStateTransitionCommand
                {
                    FlowNodeElementInstanceId = flowNodeInstanceId,
                    FlowNodeInstanceId = processInstanceId,
                    Parameters = jObj,
                    State = state
                }, token);
            }

            public Task EnqueueMessage(string processInstanceId, string messageName, object jObj, CancellationToken token)
            {
                var content = jObj == null ? null : JObject.FromObject(jObj);
                return _mediator.Send(new ConsumeMessageInstanceCommand
                {
                    FlowNodeInstanceId = processInstanceId,
                    MessageContent = content,
                    Name = messageName
                });
            }
        }

        private class FakeHttpClientFactory : IHttpClientFactory
        {
            public FakeHttpClientFactory()
            {
                MockHttpHandler = new Mock<HttpMessageHandler>();
            }

            public HttpClient Build()
            {
                return new HttpClient(MockHttpHandler.Object);
            }

            public Mock<HttpMessageHandler> MockHttpHandler { get; private set; }
        }
    }

    public interface IHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
