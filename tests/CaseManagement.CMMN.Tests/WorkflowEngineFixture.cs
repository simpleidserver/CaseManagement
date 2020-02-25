using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.CasePlanInstance.Processors.CaseFileItem;
using CaseManagement.CMMN.CasePlanInstance.Repositories;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.CMMN.Tests.Delegates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNTask("1", "First Task", (c) => {
                    c.SetManualActivationRule("activation", new CMMNExpression("language", "true"));
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List <IProcessor>
            {
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Enabled);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Enabled), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(3).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("increment");
                    c.SetIsBlocking(true);
                    c.AddMapping("increment", "increment");
                    c.SetRepetitionRule("activation", new CMMNExpression("language", "context.GetNumberVariable(\"increment\") &lt;= 2"));
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(3, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
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
                        s.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.SetRepetitionRule("activation", null);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(4, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Active);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Suspend);
            Wait(workflowInstance, "1", TaskStates.Suspended);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Resume);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
        }

        [Fact]
        public void When_Execute_One_Task_And_Disable()
        {
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNTask("1", "First Task", (c) => {
                    c.SetManualActivationRule("activation", new CMMNExpression("language", "true"));
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Enabled);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Disable);
            Wait(workflowInstance, "1", TaskStates.Disabled);
            var ex = Assert.Throws<AggregateValidationException>(() => workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart));
            Assert.NotNull(ex);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Reenable);
            Wait(workflowInstance, "1", TaskStates.Enabled);
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.ManualStart);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Enabled), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Disabled), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Enabled), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(5).State);

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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Active);
            var elt = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNTransitions.Terminate);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one failed task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("failed");
                    c.SetIsBlocking(true);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Failed);
            workflowEngine.Reactivate(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Failed, 2);

            var workflowInstanceStateHistories = workflowInstance.StateHistories.ToList();
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstanceStateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Failed), workflowInstanceStateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstanceStateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Failed), workflowInstanceStateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
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
                        d.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                        d.SetIfPart("context.GetNumberVariable(\"increment\") == 1");
                    });
                })
                .AddCMMNTask("3", "Third task", (c) =>
                {
                    c.SetIsBlocking(true);
                    c.AddEntryCriterion("entry", (d) =>
                    {
                        d.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                        d.SetIfPart("context.GetNumberVariable(\"increment\") == 2");
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Completed);
            Wait(workflowInstance, "2", TaskStates.Completed);
            Wait(workflowInstance, "3", TaskStates.Available);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CaseStates.Terminated);
            
            Assert.Equal(3, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNTask("1", "First task", (cb) => { })
                .AddCMMNProcessTask("2", "Second Task", (c) => {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                    c.AddEntryCriterion("entry", (cb) =>
                    {
                        cb.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Start });
                    });
                    c.AddExitCriterion("exit", (cb) =>
                    {
                        cb.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(2000);
            // Wait(workflowInstance, CMMNCaseStates.Completed);

            Assert.Equal(2, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "1").TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.Start, workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "1").TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.Complete, workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "1").TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "1").StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "1").StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "1").StateHistories.ElementAt(2).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "2").TransitionHistories.ElementAt(0).Transition);
            Assert.True(workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "2").TransitionHistories.Any(t => t.Transition == CMMNTransitions.Terminate) == true);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "2").StateHistories.ElementAt(0).State);
            Assert.True(workflowInstance.WorkflowElementInstances.First(i => i.CasePlanElementId == "2").StateHistories.Any(t => t.State == (Enum.GetName(typeof(TaskStates), TaskStates.Terminated))) == true);
        }

        #endregion

        #region Human task

        [Fact]
        public void When_Execute_OneHumanTask()
        {
            var serviceCollection = new ServiceCollection();
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNHumanTask("1", "First Task", (c) => {
                    c.SetIsBlocking(true);
                    c.SetFormId("form");
                    c.SetPerformerRef("performer");
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var formQueryRepository = new Mock<IFormQueryRepository>();
            var options = new Mock<IOptions<CMMNServerOptions>>();
            options.Setup(o => o.Value).Returns(new CMMNServerOptions());
            formQueryRepository.Setup(o => o.FindLatestVersion(It.IsAny<string>())).Returns(Task.FromResult(new FormAggregate()));
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNHumanTaskProcessor(options.Object, commitAggregateHelper.Object, formQueryRepository.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Active);
            var firstElement = workflowInstance.WorkflowElementInstances.First();
            workflowInstance.MakeTransition(firstElement.Id, CMMNTransitions.Complete);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNTask("1", "First task", (c) => { })
                .AddCMMNProcessTask("2", "Second Task", (c) => {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .AddExitCriteria("exit", (cb) =>
                {
                    cb.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Terminated);

            Assert.Equal(2, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.True(workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.Any(st => st.State == Enum.GetName(typeof(TaskStates), TaskStates.Terminated)) == true);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("increment");
                    c.SetIsBlocking(true);
                    c.AddMapping("increment", "increment");
                })
                .AddCMMNTask("2", "Second task", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.SetRepetitionRule("activation", null);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Active);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Active);
            workflowInstance.MakeTransition(CMMNTransitions.Suspend);
            Wait(workflowInstance, "1", TaskStates.Suspended);
            workflowInstance.MakeTransition(CMMNTransitions.Resume);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Suspended), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("failed");
                    c.SetIsBlocking(true);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Failed);
            workflowEngine.Reactivate(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Failed, 2);

            var workflowInstanceStateHistories = workflowInstance.StateHistories.ToList();
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstanceStateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Failed), workflowInstanceStateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstanceStateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Failed), workflowInstanceStateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetProcessRef("long");
                    c.SetIsBlocking(true);
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);
            workflowInstance.MakeTransition(CMMNTransitions.Close);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Closed), workflowInstance.StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNTask("1", "First task", (cb) => { })
                .AddCMMNStage("2", "First stage", (c) => {
                    c.AddEntryCriterion("entry", (cb) =>
                    {
                        cb.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Start });
                    });
                    c.AddExitCriterion("exit", (cb) =>
                    {
                        cb.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                    c.AddCMMNProcessTask("3", "Second task", (cb) => {
                        cb.SetProcessRef("long");
                        cb.SetIsBlocking(true);
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNTaskProcessor(commitAggregateHelper.Object),
                new CMMNStageProcessor(new Mock<ILogger<CMMNStageProcessor>>().Object, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.ElementAt(0).TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.Start, workflowInstance.WorkflowElementInstances.ElementAt(0).TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.Complete, workflowInstance.WorkflowElementInstances.ElementAt(0).TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.ElementAt(1).TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.Start, workflowInstance.WorkflowElementInstances.ElementAt(1).TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.Terminate, workflowInstance.WorkflowElementInstances.ElementAt(1).TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.ElementAt(2).TransitionHistories.ElementAt(0).Transition);
            Assert.True(workflowInstance.WorkflowElementInstances.ElementAt(2).TransitionHistories.Any(t => t.Transition == CMMNTransitions.ParentTerminate) == true);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.True(workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.Any(t => t.State == Enum.GetName(typeof(TaskStates), TaskStates.Terminated)) == true);
        }

        [Fact]
        public void When_Execute_Stage_With_One_Task()
        {
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNStage("1", "Stage", (c) => {
                    c.AddCMMNTask("2", "task", (d) =>
                    {

                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNTaskProcessor(commitAggregateHelper.Object),
                new CMMNStageProcessor(new Mock<ILogger<CMMNStageProcessor>>().Object, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
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
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNStageProcessor(new Mock<ILogger<CMMNStageProcessor>>().Object, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(4, workflowInstance.WorkflowElementInstances.Count());
            Assert.Equal(2, workflowInstance.ExecutionHistories.Count());
            Assert.NotNull(workflowInstance.ExecutionHistories.First());
            Assert.NotNull(workflowInstance.ExecutionHistories.Last());
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(2).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNStage("1", "Stage", (c) => {
                    c.AddCMMNProcessTask("2", "First Task", (d) =>
                    {
                        d.SetProcessRef("failed");
                        d.SetIsBlocking(true);
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNStageProcessor(new Mock<ILogger<CMMNStageProcessor>>().Object, commitAggregateHelper.Object),
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", TaskStates.Failed);
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.First().Id, CMMNTransitions.Reactivate);
            Wait(workflowInstance, "1", TaskStates.Failed, 2);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CaseStates.Terminated);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(4).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(4).State);
        }

        #endregion

        #region Milestone

        [Fact]
        public void When_Complete_Milestone()
        {
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNTask("1", "First Task", (c) => { })
                .AddCMMNMilestone("2", "Milestone", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNTaskProcessor(commitAggregateHelper.Object),
                new CMMNMilestoneProcessor()
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
        }

        [Fact]
        public void When_Terminate_Milestone()
        {
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNMilestone("1", "Milestone", (c) => { })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNMilestoneProcessor()
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available");
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.First().Id, CMMNTransitions.Terminate);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Terminated), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
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
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCMMNProcessTask("1", "First Task", (c) =>
                {
                    c.SetIsBlocking(true);
                    c.SetProcessRef("long");
                })
                .AddCMMNMilestone("2", "Milestone", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.Complete });
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNTaskProcessor(commitAggregateHelper.Object),
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object),
                new CMMNMilestoneProcessor()
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "2", "Available");
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Suspend);
            Wait(workflowInstance, "2", "Suspended");
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Resume);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(3).State);
        }

        #endregion

        #region Timer event listener

        [Fact]
        public void When_Complete_TimerEventListener()
        {
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one TimerEventListener", string.Empty, caseFile)
                .AddTimerEventListener("1", "Timer", (c) => {
                    c.SetTimer(new CMMNExpression { Language = "", Body = "R5/P0Y0M0DT0H0M2S" });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNTimerEventListenerProcessor()
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Completed);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(2).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(3).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(4).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(4).StateHistories.ElementAt(1).State);
        }

        [Fact]
        public void When_Suspend_And_Resume_TimerEventListener()
        {
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one TimerEventListener", string.Empty, caseFile)
                .AddTimerEventListener("1", "Timer", (c) => {
                    c.SetTimer(new CMMNExpression { Language = "", Body = "R2/P0Y0M0DT0H0M4S" });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNTimerEventListenerProcessor()
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available", 2);
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Suspend);
            Wait(workflowInstance, "1", "Suspended");
            Wait(workflowInstance, "1", "Completed");
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            workflowInstance.MakeTransition(workflowInstance.WorkflowElementInstances.Last().Id, CMMNTransitions.Resume);
            Wait(workflowInstance, CaseStates.Completed);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Completed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(0).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Suspended), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Available), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(MilestoneStates), MilestoneStates.Completed), workflowInstance.WorkflowElementInstances.ElementAt(1).StateHistories.ElementAt(3).State);
        }

        #endregion

        #region CaseFileItem

        [Fact]
        public void When_Add_CaseFileItem()
        {
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one CaseFileItem", string.Empty, caseFile)
                .AddCaseFileItem("1", "1", (cb) =>
                {
                    cb.SetDefinition("https://github.com/simpleidserver/casemanagement/directory");
                })
                .AddCMMNTask("2", "2", (cb) =>
                {
                    cb.AddEntryCriterion("entry", (c) =>
                    {
                        c.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.AddChild });
                    });
                })
                .Build();
            var repository = new InMemoryDirectoryCaseFileItemRepository();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNCaseFileItemProcessor(new []
                {
                    new DirectoryCaseFileItemListener(repository)
                }),
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available");
            var caseFileItemInstance = repository.FindByCaseElementInstance(workflowInstance.WorkflowElementInstances.First().Id).Result;
            var tmpPath = Path.Combine(caseFileItemInstance.Value, "tmp.txt");
            File.WriteAllText(tmpPath, "tmp");
            Wait(workflowInstance, "2", TaskStates.Completed);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CaseStates.Terminated);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Read_CaseFileItem()
        {
            var serviceCollection = new ServiceCollection();
            var repository = new InMemoryDirectoryCaseFileItemRepository();
            serviceCollection.AddSingleton<ICaseFileItemRepository>(repository);
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(ReadCasefileTask).AssemblyQualifiedName,
                    Id = "readcase"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCaseFileItem("1", "1", (cb) =>
                {
                    cb.SetDefinition("https://github.com/simpleidserver/casemanagement/directory");
                })
                .AddCMMNProcessTask("2", "First Task", (c) => {
                    c.SetProcessRef("readcase");
                    c.SetIsBlocking(true);
                    c.AddCaseInstanceIdInputMapping();
                    c.AddMapping("contentFile", "contentFile");
                    c.AddEntryCriterion("entry", (cb) =>
                    {
                        cb.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.AddChild });
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNCaseFileItemProcessor(new []
                {
                    new DirectoryCaseFileItemListener(repository)
                }),
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available");
            var caseFileItemInstance = repository.FindByCaseElementInstance(workflowInstance.WorkflowElementInstances.First().Id).Result;
            var tmpPath = Path.Combine(caseFileItemInstance.Value, "tmp.txt");
            File.WriteAllText(tmpPath, "tmp");
            Wait(workflowInstance, "2", TaskStates.Completed);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CaseStates.Terminated);
            Assert.Equal("tmp", workflowInstance.ExecutionContext.Variables["contentFile"]);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Terminated), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.AddChild, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.ParentTerminate, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(Enum.GetName(typeof(CaseFileItemStates), CaseFileItemStates.Available), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseFileItemStates), CaseFileItemStates.Available), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CaseFileItemStates), CaseFileItemStates.Available), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(2).State);
        }

        [Fact]
        public void When_Reactivate_CaseFileItem()
        {
            var serviceCollection = new ServiceCollection();
            var repository = new InMemoryDirectoryCaseFileItemRepository();
            serviceCollection.AddSingleton<ICaseFileItemRepository>(repository);
            var processQueryRepository = new InMemoryProcessQueryRepository(new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    AssemblyQualifiedName = typeof(FailedTask).AssemblyQualifiedName,
                    Id = "failedtask"
                }
            });
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(processQueryRepository, new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCaseFileItem("1", "1", (cb) =>
                {
                    cb.SetDefinition("https://github.com/simpleidserver/casemanagement/directory");
                })
                .AddCMMNProcessTask("2", "First Task", (c) => {
                    c.SetProcessRef("failedtask");
                    c.SetIsBlocking(true);
                    c.AddEntryCriterion("entry", (cb) =>
                    {
                        cb.AddOnPart(new CaseFileItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.AddChild });
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNCaseFileItemProcessor(new []
                {
                    new DirectoryCaseFileItemListener(repository)
                }),
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available");
            var caseFileItemInstance = repository.FindByCaseElementInstance(workflowInstance.WorkflowElementInstances.First().Id).Result;
            var tmpPath = Path.Combine(caseFileItemInstance.Value, "tmp.txt");
            File.WriteAllText(tmpPath, "tmp");
            Wait(workflowInstance, "2", TaskStates.Failed);
            workflowEngine.Reactivate(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, CaseStates.Failed, 2);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Failed), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Failed), workflowInstance.StateHistories.ElementAt(3).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.AddChild, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(Enum.GetName(typeof(CaseFileItemStates), CaseFileItemStates.Available), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseFileItemStates), CaseFileItemStates.Available), workflowInstance.WorkflowElementInstances.First().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(3).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Failed), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(4).State);
        }

        [Fact]
        public void When_Resume_CaseFileItem()
        {
            var repository = new InMemoryDirectoryCaseFileItemRepository();
            var caseFile = CaseFileAggregate.New("file", "file", 0, string.Empty);
            var workflowDefinition = WorkflowBuilder.New("templateId", "Case with one task", string.Empty, caseFile)
                .AddCaseFileItem("1", "1", (cb) =>
                {
                    cb.SetDefinition("https://github.com/simpleidserver/casemanagement/directory");
                })
                .AddCMMNTask("2", "First Task", (c) => {
                    c.SetIsBlocking(true);
                    c.AddEntryCriterion("entry", (cb) =>
                    {
                        cb.AddOnPart(new PlanItemOnPart { SourceRef = "1", StandardEvent = CMMNTransitions.AddChild });
                    });
                })
                .Build();
            var logger = new Mock<ILogger<CaseEngine>>();
            var commitAggregateHelper = new Mock<ICommitAggregateHelper>();
            var workflowEngine = new CaseEngine(logger.Object, new List<IProcessor>
            {
                new CMMNCaseFileItemProcessor(new []
                {
                    new DirectoryCaseFileItemListener(repository)
                }),
                new CMMNTaskProcessor(commitAggregateHelper.Object)
            });
            var workflowInstance = Domains.CasePlanInstanceAggregate.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Wait(workflowInstance, "1", "Available");
            workflowInstance.MakeTransition(CMMNTransitions.Suspend);
            Wait(workflowInstance, CaseStates.Suspended);
            workflowInstance.MakeTransition(CMMNTransitions.Resume);
            Wait(workflowInstance, CaseStates.Active);            
            var caseFileItemInstance = repository.FindByCaseElementInstance(workflowInstance.WorkflowElementInstances.First().Id).Result;
            var tmpPath = Path.Combine(caseFileItemInstance.Value, "tmp.txt");
            File.WriteAllText(tmpPath, "tmp");
            Wait(workflowInstance, "2", TaskStates.Completed);
            workflowInstance.MakeTransition(CMMNTransitions.Terminate);
            Wait(workflowInstance, CaseStates.Terminated);

            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Suspended), workflowInstance.StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Active), workflowInstance.StateHistories.ElementAt(2).State);
            Assert.Equal(Enum.GetName(typeof(CaseStates), CaseStates.Terminated), workflowInstance.StateHistories.ElementAt(3).State);
            Assert.Equal(CMMNTransitions.Create, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(0).Transition);
            Assert.Equal(CMMNTransitions.ParentSuspend, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(1).Transition);
            Assert.Equal(CMMNTransitions.AddChild, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(2).Transition);
            Assert.Equal(CMMNTransitions.ParentTerminate, workflowInstance.WorkflowElementInstances.First().TransitionHistories.ElementAt(3).Transition);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Available), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(0).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Active), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(1).State);
            Assert.Equal(Enum.GetName(typeof(TaskStates), TaskStates.Completed), workflowInstance.WorkflowElementInstances.Last().StateHistories.ElementAt(2).State);
        }

        #endregion
        
        private void Wait(Domains.CasePlanInstanceAggregate workflowInstance, CaseStates caseState, int nbInstances = 1)
        {
            var lst = workflowInstance.StateHistories.ToList().Where(c => c.State == Enum.GetName(typeof(CaseStates), caseState));
            if (lst.Count() < nbInstances)
            {
                Thread.Sleep(MS);
                Wait(workflowInstance, caseState, nbInstances);
            }
        }

        private void Wait(Domains.CasePlanInstanceAggregate workflowInstance, string eltDefId, TaskStates state, int nbInstances = 1)
        {
            Wait(workflowInstance, eltDefId, Enum.GetName(typeof(TaskStates), state));
        }

        private void Wait(Domains.CasePlanInstanceAggregate workflowInstance, string eltDefId, string state, int nbInstances = 1)
        {
            var instances = workflowInstance.WorkflowElementInstances.Where(w => w.CasePlanElementId == eltDefId);
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
