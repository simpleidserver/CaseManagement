using CaseManagement.CMMN.CasePlanInstance.Processors.Builders;
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
                        __.AddPlanItemOnPart("2", Domains.CMMNTransitions.Complete);
                        __.AddPlanItemOnPart("3", Domains.CMMNTransitions.Complete);
                    });
                })
                .Build();
            var jobServer = FakeCaseJobServer.New();
            try
            {
                await jobServer.RegisterCasePlanInstance(instance, CancellationToken.None);
                jobServer.Start();
                await jobServer.EnqueueCasePlanInstance("1");
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    return c.State == CaseStates.Completed;
                });
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
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
                await jobServer.EnqueueCasePlanInstance("1");
                await jobServer.PublishExternalEvt("complete", "1", "2");
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    return c.State == CaseStates.Completed;
                });
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
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
                await jobServer.EnqueueCasePlanInstance("1");
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    return c.State == CaseStates.Completed;
                });
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
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
                await jobServer.EnqueueCasePlanInstance("1");
                await jobServer.PublishExternalEvt("manualstart", "1", "2");
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    return c.State == CaseStates.Completed;
                });
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
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
                await jobServer.EnqueueCasePlanInstance("1");
                await jobServer.PublishExternalEvt("terminate", "1");
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    return c.State == CaseStates.Terminated;
                });
                var child = casePlanInstance.Content.Children.First() as HumanTaskElementInstance;
                Assert.Equal(CaseStates.Terminated, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Terminated, child.State);
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
                await jobServer.EnqueueCasePlanInstance("1");
                await jobServer.PublishExternalEvt("terminate", "1", "2");
                var casePlanInstance = await jobServer.MonitorCasePlanInstance("1", (c) =>
                {
                    if (!c.Content.Children.Any())
                    {
                        return false;
                    }

                    var ht = c.Content.Children.First() as HumanTaskElementInstance;
                    return ht.State == TaskStageStates.Terminated;
                });
                var child = casePlanInstance.Content.Children.First() as HumanTaskElementInstance;
                Assert.Equal(CaseStates.Completed, casePlanInstance.State);
                Assert.Equal(TaskStageStates.Terminated, child.State);
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

            public Task RegisterCasePlanInstance(CasePlanInstanceAggregate casePlanInstance, CancellationToken cancellationToken)
            {
                return _caseJobServer.RegisterCasePlanInstance(casePlanInstance, cancellationToken);
            }

            public Task PublishExternalEvt(string evt, string casePlanInstanceId, string casePlanElementInstanceId = null)
            {
                return _caseJobServer.PublishExternalEvt(evt, casePlanInstanceId, casePlanElementInstanceId);
            }

            public Task EnqueueCasePlanInstance(string id)
            {
                return _caseJobServer.EnqueueCasePlanInstance(id);
            }

            public async Task<CasePlanInstanceAggregate> MonitorCasePlanInstance(string id, Func<CasePlanInstanceAggregate, bool> callback)
            {
                while(true)
                {
                    Thread.Sleep(100);
                    var result = await _casePlanInstanceQueryRepository.Get(id);
                    if (callback(result))
                    {
                        return result;
                    }
                }
            }
        }
    }
}
