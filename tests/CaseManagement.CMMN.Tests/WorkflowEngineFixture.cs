using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseInstance.Processors.CaseFileItem;
using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.CMMN.Tests.Delegates;
using CaseManagement.Workflow.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class WorkflowEngineFixture
    {
        private const int MS = 400;

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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Enabled);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart);
            Wait(workflowInstance, CMMNCaseStates.Completed);
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);

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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Active);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Suspend);
            Wait(workflowInstance, "1", CMMNTaskStates.Suspended);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Resume);
            Wait(workflowInstance, CMMNCaseStates.Completed);
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Enabled);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Disable);
            Wait(workflowInstance, "1", CMMNTaskStates.Disabled);
            var ex = Assert.Throws<AggregateValidationException>(() => workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart));
            Assert.NotNull(ex);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Reenable);
            Wait(workflowInstance, "1", CMMNTaskStates.Enabled);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart);
            Wait(workflowInstance, CMMNCaseStates.Completed);
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Active);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Terminate);
            Wait(workflowInstance, CMMNCaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Reactivate_FailTask()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(FailedTask).AssemblyQualifiedName,
                    Id = "failed"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one failed task")
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("failed");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Failed);
            workflowEngine.Reactivate(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Failed, 2);

            var workflowInstanceStateHistories = workflowInstance.StateHistories.ToList();
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstanceStateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Failed), workflowInstanceStateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstanceStateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Failed), workflowInstanceStateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
        }

        #endregion

        #region Entry criterias

        [Fact]
        public void When_Execute_Tasks_With_EntryCriterias()
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Completed);
            Wait(workflowInstance, "2", CMMNTaskStates.Completed);
            Wait(workflowInstance, "3", CMMNTaskStates.Available);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CMMNCaseStates.Terminated);
            
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

        #region Exit criterias

        [Fact]
        public void When_Execute_Task_With_ExitCriteria()
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
                .AddCMMNTask("1", "First task", (cb) => { })
                .AddCMMNProcessTask("2", "Second Task", (c) => {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                    c.AddEntryCriterion("entry", (cb) =>
                    {
                        cb.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Start });
                    });
                    c.AddExitCriterion("exit", (cb) =>
                    {
                        cb.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2000);
            // Wait(workflowInstance, CMMNCaseStates.Completed);

            Assert.Equal(2, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "1").TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.Start, workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "1").TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.Complete, workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "1").TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "1").StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "1").StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "1").StateHistories.ElementAt(2).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "2").TransitionHistories.ElementAt(0).Transition);
            Assert.True(workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "2").TransitionHistories.Any(t => t.Transition == CMMNTransitions.Exit) == true);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "2").StateHistories.ElementAt(0).State);
            Assert.True(workflowInstance.WorkflowElementInstances.First(i => i.WorkflowElementDefinitionId == "2").StateHistories.Any(t => t.State == (Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated))) == true);
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNHumanTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Active);
            var firstElement = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.SubmitForm(firstElement.Id, firstElement.FormInstanceId);
            Wait(workflowInstance, CMMNCaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
        }

        #endregion

        #endregion

        #region Case instance

        [Fact]
        public void When_Execute_CaseInstance_With_Exit_Criteria()
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
                .AddCMMNTask("1", "First task", (c) => { })
                .AddCMMNProcessTask("2", "Second Task", (c) => {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .AddExitCriteria("exit", (cb) =>
                {
                    cb.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Terminated);

            Assert.Equal(2, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.True(workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.Any(st => st.State == Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated)) == true);
        }

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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);

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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Active);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Active);
            workflowInstance.MakeTransition(CMMNTransitions.Suspend);
            Wait(workflowInstance, "1", CMMNTaskStates.Suspended);
            workflowInstance.MakeTransition(CMMNTransitions.Resume);
            Wait(workflowInstance, CMMNCaseStates.Completed);

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

        [Fact]
        public void When_Reactivate_Failed_CaseInstance()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(FailedTask).AssemblyQualifiedName,
                    Id = "failed"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("failed");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Failed);
            workflowEngine.Reactivate(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Failed, 2);

            var workflowInstanceStateHistories = workflowInstance.StateHistories.ToList();
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstanceStateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Failed), workflowInstanceStateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstanceStateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Failed), workflowInstanceStateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
        }

        [Fact]
        public void When_Close_CaseInstance()
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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);
            workflowInstance.MakeTransition(CMMNTransitions.Close);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Closed), workflowInstance.StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
        }

        #endregion

        #region Stage

        [Fact]
        public void When_Execute_Stage_With_Exit_Criteria()
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
                .AddCMMNTask("1", "First task", (cb) => { })
                .AddCMMNStage("2", "First stage", (c) => {
                    c.AddEntryCriterion("entry", (cb) =>
                    {
                        cb.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Start });
                    });
                    c.AddExitCriterion("exit", (cb) =>
                    {
                        cb.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.AddCMMNProcessTask("3", "Second task", (cb) => {
                        cb.SetProcessRef("long");
                        cb.SetIsBlocking(true);
                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNTaskProcessor(),
                new CMMNStageProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.ElementAt(0).TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.Start, workflowInstance.WorkflowElementInstances.ElementAt(0).TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.Complete, workflowInstance.WorkflowElementInstances.ElementAt(0).TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.ElementAt(1).TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.Start, workflowInstance.WorkflowElementInstances.ElementAt(1).TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.Exit, workflowInstance.WorkflowElementInstances.ElementAt(1).TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.ElementAt(2).TransitionHistories.ElementAt(0).Transition);
            Assert.True(workflowInstance.WorkflowElementInstances.ElementAt(2).TransitionHistories.Any(t => t.Transition == CMMNTransitions.ParentExit) == true);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.True(workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.Any(t => t.State == Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Terminated)) == true);
        }

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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNTaskProcessor(),
                new CMMNStageProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);

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
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNStageProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);
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

        [Fact]
        public void When_Reactivate_Failed_Stage()
        {
            var serviceCollection = new ServiceCollection();
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(FailedTask).AssemblyQualifiedName,
                    Id = "failed"
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
                        d.SetProcessRef("failed");
                        d.SetIsBlocking(true);
                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNStageProcessor(),
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", CMMNTaskStates.Failed);
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.First().Id, CMMNTransitions.Reactivate);
            Wait(workflowInstance, "1", CMMNTaskStates.Failed, 2);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CMMNCaseStates.Terminated);

            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(4).State);
        }

        #endregion

        #region Milestone

        [Fact]
        public void When_Complete_Milestone()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNTask("1", "First Task", (c) => { })
                .AddCMMNMilestone("2", "Milestone", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNTaskProcessor(),
                new CMMNMilestoneProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
        }

        [Fact]
        public void When_Terminate_Milestone()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one task")
                .AddCMMNMilestone("1", "Milestone", (c) => { })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNMilestoneProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available");
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.First().Id, CMMNTransitions.Terminate);
            Wait(workflowInstance, CMMNCaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
        }

        [Fact]
        public void When_Suspend_And_Resume_Milestone()
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
                    c.SetIsBlocking(true);
                    c.SetProcessRef("long");
                })
                .AddCMMNMilestone("2", "Milestone", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNTaskProcessor(),
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler),
                new CMMNMilestoneProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "2", "Available");
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Suspend);
            Wait(workflowInstance, "2", "Suspended");
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Resume);
            Wait(workflowInstance, CMMNCaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(3).State);
        }

        #endregion

        #region Timer event listener

        [Fact]
        public void When_Complete_TimerEventListener()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one TimerEventListener")
                .AddTimerEventListener("1", "Timer", (c) => {
                    c.SetTimer(new CMMNExpression { Language = "", Body = "R5/P0Y0M0DT0H0M2S" });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNTimerEventListenerProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CMMNCaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(4).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(4).StateHistories.ElementAt(1).State);
        }

        [Fact]
        public void When_Suspend_And_Resume_TimerEventListener()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one TimerEventListener")
                .AddTimerEventListener("1", "Timer", (c) => {
                    c.SetTimer(new CMMNExpression { Language = "", Body = "R2/P0Y0M0DT0H0M4S" });
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNTimerEventListenerProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available", 2);
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Suspend);
            Wait(workflowInstance, "1", "Suspended");
            Wait(workflowInstance, "1", "Completed");
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Resume);
            Wait(workflowInstance, CMMNCaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CMMNMilestoneStates), CMMNMilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(3).State);
        }

        #endregion

        #region CaseFileItem

        [Fact]
        public void When_Add_CaseFileItem()
        {
            var workflowDefinition = CMMNWorkflowBuilder.New("templateId", "Case with one CaseFileItem")
                .AddCaseFileItem("1", "1", (cb) =>
                {
                    cb.SetDefinition("https://github.com/simpleidserver/casemanagement/directory");
                })
                .AddCMMNTask("2", "2", (cb) =>
                {
                    cb.AddEntryCriterion("entry", (c) =>
                    {
                        c.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.AddChild });
                    });
                })
                .Build();
            var repository = new InMemoryDirectoryCaseFileItemRepository();
            var workflowEngine = new CMMNWorkflowEngine(new List<IProcessor>
            {
                new CMMNCaseFileItemProcessor(new []
                {
                    new DirectoryCaseFileItemListener(repository)
                }),
                new CMMNTaskProcessor()
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available");
            var caseFileItemInstance = repository.GetCaseFileItemInstance(workflowInstance.WorkflowElementInstances.First().Id).Result;
            var tmpPath = Path.Combine(caseFileItemInstance.Id, "tmp.txt");
            File.WriteAllText(tmpPath, "tmp");
            Wait(workflowInstance, "2", CMMNTaskStates.Completed);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CMMNCaseStates.Terminated);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNCaseStates), CMMNCaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Available), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Active), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CMMNTaskStates), CMMNTaskStates.Completed), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(2).State);
        }

        #endregion

        private void Wait(CMMNWorkflowInstance workflowInstance, CMMNCaseStates caseState, int nbInstances = 1)
        {
            var lst = workflowInstance.StateHistories.ToList().Where(c => c.State == Enum.GetName(typeof(CMMNCaseStates), caseState));
            if (lst.Count() < nbInstances)
            {
                Thread.Sleep(MS);
                Wait(workflowInstance, caseState, nbInstances);
            }
        }

        private void Wait(CMMNWorkflowInstance workflowInstance, string eltDefId, CMMNTaskStates state, int nbInstances = 1)
        {
            Wait(workflowInstance, eltDefId, Enum.GetName(typeof(CMMNTaskStates), state));
        }

        private void Wait(CMMNWorkflowInstance workflowInstance, string eltDefId, string state, int nbInstances = 1)
        {
            var instances = workflowInstance.WorkflowElementInstances.Where(w => w.WorkflowElementDefinitionId == eltDefId);
            if (instances.Count() < nbInstances)
            {
                Thread.Sleep(MS);
                Wait(workflowInstance, eltDefId, state);
            }

            if (!instances.Any(s => s.State == state))
            {
                Thread.Sleep(MS);
                Wait(workflowInstance, eltDefId, state);
            }
        }
    }
}
