using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.CMMN.Tests.Delegates;
using CaseManagement.Workflow.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class WorkflowEngineFixture
    {
        #region Task

        #region PlanItemControl

        [Fact]
        public void When_Execute_One_Task_With_ManualActivationRule()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNTask("1", "First Task", (c) => {
                    c.SetManualActivationRule("activation", new CMMNExpression("language", "true"));
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2 * 1000);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart);
            Thread.Sleep(2 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Enabled), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(3).State);
        }

        [Fact]
        public void When_Execute_One_ProcessTask_With_RepetitionRule()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(IncrementTask).AssemblyQualifiedName,
                    Id = "increment"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("increment");
                    c.SetIsBlocking(true);
                    c.AddMapping("increment", "increment");
                    c.SetRepetitionRule("activation", new CMMNExpression("language", "context.GetNumberVariable(\"increment\") &lt;= 2"));
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2 * 1000);
            Assert.Equal(3, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Execute_Two_Tasks_With_RepetitionRule()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(IncrementTask).AssemblyQualifiedName,
                    Id = "increment"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("increment");
                    c.SetIsBlocking(true);
                    c.AddMapping("increment", "increment");
                    c.SetRepetitionRule("activation", new CMMNExpression("language", "context.GetNumberVariable(\"increment\") &lt; 2"));
                })
                .AddCMMNTask("2", "Second task", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.SetRepetitionRule("activation", null);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(5 * 1000);
            Assert.Equal(4, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(2).State);
        }

        #endregion

        #region Transitions

        [Fact]
        public void When_Execute_LongTask_And_Suspend()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(LongTask).AssemblyQualifiedName,
                    Id = "long"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(100);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Suspend);
            Thread.Sleep(100);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Resume);
            Thread.Sleep(3 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
        }

        [Fact]
        public void When_Execute_One_Task_And_Disable()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNTask("1", "First Task", (c) => {
                    c.SetManualActivationRule("activation", new CMMNExpression("language", "true"));
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(100);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Disable);
            Thread.Sleep(100);
            var ex = Assert.Throws<AggregateValidationException>(() => workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart));
            Assert.NotNull(ex);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Reenable);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart);
            Thread.Sleep(2 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Enabled), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Disabled), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Enabled), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(5).State);
        }
        
        [Fact]
        public void When_Execute_LongTask_And_Terminate()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(LongTask).AssemblyQualifiedName,
                    Id = "long"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(100);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Terminate);
            Thread.Sleep(2 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
        }

        #endregion

        #region Entry criterias

        [Fact]
        public void When_Execute_Tasks_With_EntrtCriterias()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(IncrementTask).AssemblyQualifiedName,
                    Id = "increment"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("increment");
                    c.SetIsBlocking(true);
                    c.AddMapping("increment", "increment");
                })
                .AddCMMNTask("2", "Second task", (c) =>
                {
                    c.SetIsBlocking(true);
                    c.AddEntryCriterion("entry", (d) =>
                    {
                        d.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                        d.SetIfPart("context.GetNumberVariable(\"increment\") == 1");
                    });
                })
                .AddCMMNTask("3", "Third task", (c) =>
                {
                    c.SetIsBlocking(true);
                    c.AddEntryCriterion("entry", (d) =>
                    {
                        d.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                        d.SetIfPart("context.GetNumberVariable(\"increment\") == 2");
                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2 * 1000);
            Assert.Equal(3, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Single(workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories);
        }

        #endregion

        #region Human task

        [Fact]
        public void When_Execute_OneHumanTask()
        {
            var serviceCollection = new ServiceCollection();
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNHumanTask("1", "First Task", (c) => {
                    c.SetIsBlocking(true);
                    c.SetFormId("form");
                    c.SetPerformerRef("performer");
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNHumanTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(1 * 1000);
            var firstElement = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.SubmitForm(firstElement.Id, firstElement.FormInstanceId);
            Thread.Sleep(1 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
        }

        #endregion

        #endregion

        #region Case instance

        [Fact]
        public void When_Complete_CaseInstance()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(IncrementTask).AssemblyQualifiedName,
                    Id = "increment"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("increment");
                    c.SetIsBlocking(true);
                    c.AddMapping("increment", "increment");
                })
                .AddCMMNTask("2", "Second task", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.SetRepetitionRule("activation", null);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Terminate_CaseInstance()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(LongTask).AssemblyQualifiedName,
                    Id = "long"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(100);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Thread.Sleep(2 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Suspend_CaseInstance()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(LongTask).AssemblyQualifiedName,
                    Id = "long"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(100);
            workflowInstance.MakeTransition(CMMNTransitions.Suspend);
            Thread.Sleep(3 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Suspended), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Reactivate_CaseInstance()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(LongTask).AssemblyQualifiedName,
                    Id = "long"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(100);
            workflowInstance.MakeTransition(CMMNTransitions.Suspend);
            Thread.Sleep(100);
            workflowInstance.MakeTransition(CMMNTransitions.Reactivate);
            Thread.Sleep(2 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Suspended), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
        }

        #endregion

        #region Stage

        // Tester un stage qui se trouve dans un case

        // Tester l'enchaînement de plusieurs stages.

        [Fact]
        public void When_Execute_Stage_With_One_Task()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNStage("1", "Stage", (c) => {
                    c.AddCMMNTask("2", "task", (d) =>
                    {

                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNTaskProcessor(),
                new CMMNStageProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2 * 1000);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Execute_Stage_With_One_Task_With_RepetitionRule()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(IncrementTask).AssemblyQualifiedName,
                    Id = "increment"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNStage("1", "Stage", (c) => {
                    c.AddCMMNProcessTask("2", "First Task", (d) =>
                    {
                        d.SetProcessRef("increment");
                        d.SetIsBlocking(true);
                        d.AddMapping("increment", "increment");
                        d.SetRepetitionRule("activation", new CMMNExpression("language", "context.GetNumberVariable(\"increment\") &lt;= 2"));
                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<ICMMNPlanItemProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNStageProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2 * 1000);
            Assert.Equal(4, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(2, workflowInstance.ExecutionHistories.Count());
            Assert.NotNull(workflowInstance.ExecutionHistories.First());
            Assert.NotNull(workflowInstance.ExecutionHistories.Last());
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(2).State);
        }

        #endregion
    }
}
