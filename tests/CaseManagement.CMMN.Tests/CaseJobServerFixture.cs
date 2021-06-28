using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.CasePlanInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Factories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class CaseJobServerFixture
    {
        #region Case plan items

        [Fact]
        public async Task When_Execute_Empty_Tasks()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddEmptyTask("2", "firstTask", (_) =>
                {
                })
                .AddEmptyTask("3", "secondTask", (_) =>
                {
                })
                .AddEmptyTask("4", "thirdTask", (_) =>
                {
                    _.AddEntryCriteria("entry", (__) =>
                    {
                        __.AddPlanItemOnPart("2", CMMNTransitions.Complete);
                        __.AddPlanItemOnPart("3", CMMNTransitions.Complete);
                    });
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0);
            var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(1);
            var thirdEmptyTask = casePlanInstance.StageContent.Children.ElementAt(2);
            Assert.Equal(CaseStates.Completed, casePlanInstance.State);
            Assert.Equal(TaskStageStates.Completed, firstEmptyTask.TakeStageState);
            Assert.Equal(TaskStageStates.Completed, secondEmptyTask.TakeStageState);
            Assert.Equal(TaskStageStates.Completed, thirdEmptyTask.TakeStageState);
        }

        [Fact]
        public async Task When_Execute_Human_Task()
        {
            var humanTaskInstanceId = Guid.NewGuid().ToString();
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddHumanTask("2", "humanTask", null, (_) =>
                {

                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            jobServer.HttpMessageHandler.Protected()
                .As<IHttpMessageHandler>()
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{ 'id' : '{" + humanTaskInstanceId + "}' }")
                });
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            await jobServer.PublishExternalEvt("complete", "1", CaseEltInstance.BuildId("1", "2", 0), new Dictionary<string, string> { { "firstname", "firstname" } }, CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstHumanTask = casePlanInstance.StageContent.Children.ElementAt(0);
            Assert.Equal(CaseStates.Completed, casePlanInstance.State);
            Assert.Equal(TaskStageStates.Completed, firstHumanTask.TakeStageState);
        }

        [Fact]
        public async Task When_Execute_Stage()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddStage("2", "firstStage", (_) =>
                {
                    _.AddStage("3", "secondStage", (__) =>
                    {
                        __.AddEmptyTask("4", "emtpytask");
                    });
                })
                .AddStage("5", "thirdStage", (_) =>
                {
                    _.AddEmptyTask("6", "emtpytask");
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstStage = casePlanInstance.StageContent.Children.ElementAt(0);
            var secondStage = casePlanInstance.StageContent.Children.ElementAt(1);
            Assert.Equal(CaseStates.Completed, casePlanInstance.State);
            Assert.Equal(TaskStageStates.Completed, firstStage.TakeStageState);
            Assert.Equal(TaskStageStates.Completed, secondStage.TakeStageState);
        }

        [Fact]
        public async Task When_Execute_Milestone()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddEmptyTask("2", "firstTask", (_) =>
                {

                })
                .AddMilestone("3", "milestone", (_) =>
                {
                    _.AddEntryCriteria("entry", (__) =>
                    {
                        __.AddPlanItemOnPart("2", CMMNTransitions.Complete);
                    });
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0);
            var firstMilestone = casePlanInstance.StageContent.Children.ElementAt(1);
            Assert.Equal(CaseStates.Completed, casePlanInstance.State);
            Assert.Equal(TaskStageStates.Completed, firstEmptyTask.TakeStageState);
            Assert.Equal(MilestoneEventStates.Completed, firstMilestone.MilestoneState);

        }

        [Fact]
        public async Task When_Execute_FileItem()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddFileItem("2", "directory", (_) =>
                {
                    _.DefinitionType = CMMNConstants.ContentManagementTypes.FAKE_CMIS_DIRECTORY;
                })
                .AddEmptyTask("3", "emptytask", (_) =>
                {
                    _.AddEntryCriteria("entry", (__) =>
                    {
                        __.AddFileItemOnPart("2", CMMNTransitions.AddChild);
                    });
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            await jobServer.PublishExternalEvt("addchild", "1", CaseEltInstance.BuildId("1", "2"), new Dictionary<string, string> { { "fileId", "file" } }, CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstFileItem = casePlanInstance.FileItems.ElementAt(0);
            var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0);
            Assert.Equal(CaseStates.Completed, casePlanInstance.State);
            Assert.Equal(CaseFileItemStates.Available, firstFileItem.FileState);
            Assert.Equal(TaskStageStates.Completed, firstEmptyTask.TakeStageState);
        }

        [Fact]
        public async Task When_Execute_Timer()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddTimerEventListener("2", "timer", (_) =>
                {
                    _.TimerExpression = new CMMNExpression("csharp", "R2/P0Y0M0DT0H0M4S");
                })
                .AddEmptyTask("3", "emptytask", (_) =>
                {
                    _.SetRepetitionRule("name", new CMMNExpression("csharp", "context.GetVariableAndIncrement(\"counter\") < 1"));
                    _.AddEntryCriteria("entry", (__) =>
                    {
                        __.AddPlanItemOnPart("2", CMMNTransitions.Occur);
                    });
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            jobServer.Start();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
            {
                if (c == null)
                {
                    return false;
                }

                if (c.StageContent.Children.Count() == 4)
                {
                    var firstTimer = c.StageContent.Children.ElementAt(0);
                    var secondTimer = c.StageContent.Children.ElementAt(2);
                    return firstTimer.MilestoneState == MilestoneEventStates.Completed && secondTimer.MilestoneState == MilestoneEventStates.Completed;
                }

                return false;
            }, CancellationToken.None);
            var firstTimerEventListener = casePlanInstance.StageContent.Children.ElementAt(0);
            var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(1);
            var secondTimerEventListener = casePlanInstance.StageContent.Children.ElementAt(2);
            var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(3);
            Assert.Equal(MilestoneEventStates.Completed, firstTimerEventListener.MilestoneState);
            Assert.Equal(TaskStageStates.Completed, firstEmptyTask.TakeStageState);
            Assert.Equal(MilestoneEventStates.Completed, secondTimerEventListener.MilestoneState);
            Assert.Equal(TaskStageStates.Completed, secondEmptyTask.TakeStageState);
            jobServer.Stop();
        }

        #endregion

        #region Items controls

        [Fact]
        public async Task When_Execute_Task_With_ManualActivationRule()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddEmptyTask("2", "firstTask", (_) =>
                {
                    _.SetManualActivationRule("name", new CMMNExpression("csharp", "true"));
                })
                .AddEmptyTask("3", "secondTask", (_) =>
                {
                })
                .AddEmptyTask("4", "thirdTask", (_) =>
                {
                    _.AddEntryCriteria("entry", (__) =>
                    {
                        __.AddPlanItemOnPart("2", CMMNTransitions.Complete);
                        __.AddPlanItemOnPart("3", CMMNTransitions.Complete);
                    });
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            await jobServer.PublishExternalEvt("manualstart", "1", CaseEltInstance.BuildId("1", "2", 0), new Dictionary<string, string>(), CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0);
            var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0);
            var thirdEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0);
            Assert.Equal(CaseStates.Completed, casePlanInstance.State);
            Assert.Equal(TaskStageStates.Completed, firstEmptyTask.TakeStageState);
            Assert.Equal(TaskStageStates.Completed, secondEmptyTask.TakeStageState);
            Assert.Equal(TaskStageStates.Completed, thirdEmptyTask.TakeStageState);
        }

        [Fact]
        public async Task When_Execute_Task_With_RepetitionRule()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddEmptyTask("2", "firstTask", (_) =>
                {
                    _.SetRepetitionRule("name", new CMMNExpression("csharp", "context.GetVariableAndIncrement(\"counter\") < 2"));
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            Assert.Equal(2, casePlanInstance.StageContent.Children.Count());
        }

        #endregion

        #region CMMN transitions

        [Fact]
        public async Task When_Terminate_CasePlanInstance()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddHumanTask("2", "humanTask", null, (_) => { })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            await jobServer.PublishExternalEvt("terminate", "1", null, new Dictionary<string, string>(), CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstHumanTask = casePlanInstance.StageContent.Children.First();
            Assert.Equal(CaseStates.Terminated, casePlanInstance.State);
            Assert.Equal(TaskStageStates.Terminated, firstHumanTask.TakeStageState);
        }

        [Fact]
        public async Task When_Terminate_CasePlanElementInstance()
        {
            var humanTaskInstanceId = Guid.NewGuid().ToString();
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddHumanTask("2", "humanTask", null, (_) => { })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            jobServer.HttpMessageHandler.Protected()
                .As<IHttpMessageHandler>()
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{ 'id' : '{" + humanTaskInstanceId + "}' }")
                });
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            await jobServer.PublishExternalEvt("terminate", "1", CaseEltInstance.BuildId("1", "2", 0), new Dictionary<string, string>(), CancellationToken.None);

            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstHumanTask = casePlanInstance.StageContent.Children.First();
            Assert.Equal(CaseStates.Completed, casePlanInstance.State);
            Assert.Equal(TaskStageStates.Terminated, firstHumanTask.TakeStageState);
        }

        #endregion

        #region Criteria

        [Fact]
        public async Task When_Execute_IfPart()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddEmptyTask("2", "emptyTask", (_) => { })
                .AddEmptyTask("3", "secondTask", (_) =>
                {
                    _.AddEntryCriteria("entry", __ =>
                    {
                        __.SetIfPart("context.GetStrVariable(\"action\") == \"secondTask\"");
                        __.AddPlanItemOnPart("2", CMMNTransitions.Complete);
                    });
                })
                .AddEmptyTask("4", "thirdTask", (_) =>
                {
                    _.AddEntryCriteria("entry", __ =>
                    {
                        __.SetIfPart("context.GetStrVariable(\"action\") == \"thirdTask\"");
                        __.AddPlanItemOnPart("2", CMMNTransitions.Complete);
                    });
                })
                .Build();
            instance.ExecutionContext.SetStrVariable("action", "thirdTask");
            var jobServer = FakeCaseJobServer.New();
            await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
            await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
            var casePlanInstance = await jobServer.Get("1", CancellationToken.None);
            var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0);
            var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(1);
            var thirdEmptyTask = casePlanInstance.StageContent.Children.ElementAt(2);
            Assert.Equal(TaskStageStates.Completed, firstEmptyTask.TakeStageState);
            Assert.Equal(TaskStageStates.Completed, thirdEmptyTask.TakeStageState);
            Assert.Null(secondEmptyTask.TakeStageState);
        }

        #endregion

        private class FakeCaseJobServer
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly ICasePlanInstanceCommandRepository _casePlanInstanceCommandRepository;
            private readonly MediatR.IMediator _mediator;
            private readonly IBusControl _busControl;
            private FakeHttpClientFactory _factory;

            private FakeCaseJobServer()
            {
                _factory = new FakeHttpClientFactory();
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddCaseApi(callback: opt =>
                {
                    opt.WSHumanTaskAPI = "http://localhost";
                });
                serviceCollection.AddSingleton<IHttpClientFactory>(_factory);
                serviceCollection.AddMassTransitHostedService();
                _serviceProvider = serviceCollection.BuildServiceProvider();
                _casePlanInstanceCommandRepository = _serviceProvider.GetRequiredService<ICasePlanInstanceCommandRepository>();
                _mediator = _serviceProvider.GetRequiredService<MediatR.IMediator>();
                _busControl = _serviceProvider.GetRequiredService<IBusControl>();
            }

            public Mock<HttpMessageHandler> HttpMessageHandler => _factory.MockHttpHandler;

            public static FakeCaseJobServer New()
            {
                var result = new FakeCaseJobServer();
                return result;
            }

            public void Start()
            {
                _busControl.Start();
            }

            public void Stop()
            {
                _busControl.Stop();
            }


            public Task RegisterCasePlanInstance(CasePlanInstanceAggregate casePlanInstance, CancellationToken token)
            {
                return _casePlanInstanceCommandRepository.Add(casePlanInstance, token);
            }

            public Task PublishExternalEvt(string evt, string casePlanInstanceId, string casePlanElementInstanceId, Dictionary<string, string> parameters, CancellationToken token)
            {
                IBaseRequest request = null;
                switch (evt)
                {
                    case CMMNConstants.ExternalTransitionNames.AddChild:
                        request = new AddChildCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Close:
                        request = new CloseCommand(casePlanInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Complete:
                        request = new CompleteCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Disable:
                        request = new DisableCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Occur:
                        request = new OccurCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Reactivate:
                        request = new ReactivateCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Reenable:
                        request = new ReenableCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Resume:
                        request = new ResumeCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Suspend:
                        request = new SuspendCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.Terminate:
                        request = new TerminateCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                    case CMMNConstants.ExternalTransitionNames.ManualStart:
                        request = new ActivateCommand(casePlanInstanceId, casePlanElementInstanceId)
                        {
                            Parameters = parameters
                        };
                        break;
                }

                return _mediator.Send(request, token);
            }

            public Task EnqueueCasePlanInstance(string id, CancellationToken token)
            {
                return _mediator.Send(new LaunchCaseInstanceCommand
                {
                    CasePlanInstanceId = id
                }, token);
            }

            public Task<CasePlanInstanceAggregate> Get(string id, CancellationToken token)
            {
                return _casePlanInstanceCommandRepository.Get(id, token);
            }

            public async Task<CasePlanInstanceAggregate> MonitorCasePlanInstance(string id, Func<CasePlanInstanceAggregate, bool> callback, CancellationToken token)
            {
                while (true)
                {
                    Thread.Sleep(100);
                    var result = await _casePlanInstanceCommandRepository.Get(id, token);
                    if (callback(result))
                    {
                        return result;
                    }
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
    }

    public interface IHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
