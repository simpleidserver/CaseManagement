using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }
                    return c.State == CaseStates.Completed;
                }, CancellationToken.None);
                var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0) as EmptyTaskElementInstance;
                var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(1) as EmptyTaskElementInstance;
                var thirdEmptyTask = casePlanInstance.StageContent.Children.ElementAt(2) as EmptyTaskElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Completed, firstEmptyTask.State);
                Assert.Equal(TaskStageStates.Completed, secondEmptyTask.State);
                Assert.Equal(TaskStageStates.Completed, thirdEmptyTask.State);
            }
            finally
            {
                jobServer.Stop();
            }
        }

        [Fact]
        public async Task When_Execute_Human_Task()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddHumanTask("2", "humanTask", null, (_) =>
                {

                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                await jobServer.PublishExternalEvt("complete", "1", HumanTaskElementInstance.BuildId("1", "2", 0), CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.State == CaseStates.Completed;
                }, CancellationToken.None);
                var firstHumanTask = casePlanInstance.StageContent.Children.ElementAt(0) as HumanTaskElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Completed, firstHumanTask.State);
            }
            finally
            {
                jobServer.Stop();
            }
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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.State == CaseStates.Completed;
                }, CancellationToken.None);
                var firstStage = casePlanInstance.StageContent.Children.ElementAt(0) as StageElementInstance;
                var secondStage = casePlanInstance.StageContent.Children.ElementAt(1) as StageElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Completed, firstStage.State);
                Assert.Equal(TaskStageStates.Completed, secondStage.State);
            }
            finally
            {
                jobServer.Stop();
            }
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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.State == CaseStates.Completed;
                }, CancellationToken.None);
                var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0) as EmptyTaskElementInstance;
                var firstMilestone = casePlanInstance.StageContent.Children.ElementAt(1) as MilestoneElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Completed, firstEmptyTask.State);
                Assert.Equal(MilestoneEventStates.Completed, firstMilestone.State);
            }
            finally
            {
                jobServer.Stop();
            }

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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                await jobServer.PublishExternalEvt("addchild", "1", CaseFileItemInstance.BuildId("1", "2"), CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.State == CaseStates.Completed;
                }, CancellationToken.None);
                var firstFileItem = casePlanInstance.FileItems.ElementAt(0) as CaseFileItemInstance;
                var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0) as EmptyTaskElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(CaseFileItemStates.Available, firstFileItem.State);
                Assert.Equal(TaskStageStates.Completed, firstEmptyTask.State);
            }
            finally
            {
                jobServer.Stop();
            }
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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    if (c.StageContent.Children.Count() == 4)
                    {
                        var firstTimer = c.StageContent.Children.ElementAt(0) as TimerEventListener;
                        var secondTimer = c.StageContent.Children.ElementAt(2) as TimerEventListener;
                        return firstTimer.State == MilestoneEventStates.Completed && secondTimer.State == MilestoneEventStates.Completed;
                    }

                    return false;
                }, CancellationToken.None);
                var firstTimerEventListener = casePlanInstance.StageContent.Children.ElementAt(0) as TimerEventListener;
                var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(1) as EmptyTaskElementInstance;
                var secondTimerEventListener = casePlanInstance.StageContent.Children.ElementAt(2) as TimerEventListener;
                var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(3) as EmptyTaskElementInstance;
                Assert.Equal(MilestoneEventStates.Completed, firstTimerEventListener.State);
                Assert.Equal(TaskStageStates.Completed, firstEmptyTask.State);
                Assert.Equal(MilestoneEventStates.Completed, secondTimerEventListener.State);
                Assert.Equal(TaskStageStates.Completed, secondEmptyTask.State);
            }
            finally
            {
                jobServer.Stop();
            }
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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                await jobServer.PublishExternalEvt("manualstart", "1", EmptyTaskElementInstance.BuildId("1", "2", 0), CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.State == CaseStates.Completed;
                }, CancellationToken.None);
                var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0) as EmptyTaskElementInstance;
                var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0) as EmptyTaskElementInstance;
                var thirdEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0) as EmptyTaskElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Completed, firstEmptyTask.State);
                Assert.Equal(TaskStageStates.Completed, secondEmptyTask.State);
                Assert.Equal(TaskStageStates.Completed, thirdEmptyTask.State);
            }
            finally
            {
                jobServer.Stop();
            }
        }

        [Fact]
        public async Task When_Execute_Task_With_RepetitionRule()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddEmptyTask("2", "firstTask", (_) =>
                {
                    _.SetRepetitionRule("name", new CMMNExpression("csharp", "context.GetVariableAndIncrement(\"counter\") < 1"));
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.StageContent.Children.Count() == 2;
                }, CancellationToken.None);
                Assert.Equal(2, casePlanInstance.StageContent.Children.Count());
            }
            finally
            {
                jobServer.Stop();
            }
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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                await jobServer.PublishExternalEvt("terminate", "1", null, CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    return c.State == CaseStates.Terminated;
                }, CancellationToken.None);
                var firstHumanTask = casePlanInstance.StageContent.Children.First() as HumanTaskElementInstance;
                Assert.Equal(CaseStates.Terminated, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Terminated, firstHumanTask.State);
            }
            finally
            {
                jobServer.Stop();
            }
        }

        [Fact]
        public async Task When_Terminate_CasePlanElementInstance()
        {
            var instance = CasePlanInstanceBuilder.New("1", "firstCase")
                .AddHumanTask("2", "humanTask", null, (_) => { })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                await jobServer.PublishExternalEvt("terminate", "1", HumanTaskElementInstance.BuildId("1", "2", 0), CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null || !c.StageContent.Children.Any())
                    {
                        return false;
                    }

                    var ht = c.StageContent.Children.First() as HumanTaskElementInstance;
                    return ht.State == TaskStageStates.Terminated;
                }, CancellationToken.None);
                var firstHumanTask = casePlanInstance.StageContent.Children.First() as HumanTaskElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Terminated, firstHumanTask.State);
            }
            finally
            {
                jobServer.Stop();
            }
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
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1", CancellationToken.None);
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (c == null)
                    {
                        return false;
                    }

                    if (c.StageContent.Children.Count() == 3)
                    {
                        var th = c.StageContent.Children.ElementAt(2) as EmptyTaskElementInstance;
                        if (th.State == TaskStageStates.Completed)
                        {
                            return true;
                        }
                    }

                    return false;
                }, CancellationToken.None);
                var firstEmptyTask = casePlanInstance.StageContent.Children.ElementAt(0) as EmptyTaskElementInstance;
                var secondEmptyTask = casePlanInstance.StageContent.Children.ElementAt(1) as EmptyTaskElementInstance;
                var thirdEmptyTask = casePlanInstance.StageContent.Children.ElementAt(2) as EmptyTaskElementInstance;
                Assert.Equal(TaskStageStates.Completed, firstEmptyTask.State);
                Assert.Equal(TaskStageStates.Available, secondEmptyTask.State);
                Assert.Equal(TaskStageStates.Completed, thirdEmptyTask.State);
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
            private ICaseJobServer _caseJobServer;
            private ICasePlanInstanceQueryRepository _casePlanInstanceQueryRepository;

            private FakeCaseJobServer() 
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddCaseJobServer();
                _serviceProvider = serviceCollection.BuildServiceProvider();
                _caseJobServer = _serviceProvider.GetRequiredService<ICaseJobServer>();
                _casePlanInstanceQueryRepository = _serviceProvider.GetRequiredService<ICasePlanInstanceQueryRepository>();
            }

            public static FakeCaseJobServer New()
            {
                var result = new FakeCaseJobServer();
                return result;
            }

            public void Start()
            {
                _caseJobServer.Start();
            }

            public void Stop()
            {
                _caseJobServer.Stop();
            }

            public Task RegisterCasePlanInstance(CasePlanInstanceAggregate casePlanInstance, CancellationToken token)
            {
                return _caseJobServer.RegisterCasePlanInstance(casePlanInstance, token);
            }

            public Task PublishExternalEvt(string evt, string casePlanInstanceId, string casePlanElementInstanceId, CancellationToken token)
            {
                return _caseJobServer.PublishExternalEvt(evt, casePlanInstanceId, casePlanElementInstanceId, token);
            }

            public Task EnqueueCasePlanInstance(string id, CancellationToken token)
            {
                return _caseJobServer.EnqueueCasePlanInstance(id, token);
            }

            public async Task<CasePlanInstanceAggregate> MonitorCasePlanInstance(string id, Func<CasePlanInstanceAggregate, bool> callback, CancellationToken token)
            {
                while(true)
                {
                    Thread.Sleep(100);
                    var result = await _casePlanInstanceQueryRepository.Get(id, token);
                    if (callback(result))
                    {
                        return result;
                    }
                }
            }
        }
    }
}
