using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class WorkflowEngineFixture
    {
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
            Thread.Sleep(5 * 1000);
            var elt = workflowInstance.PlanItemInstances.First();
            workflowInstance.MakeTransition(elt.Id, CMMNPlanItemTransitions.ManualStart);
        }

        [Fact]
        public void When_Execute_One_Task_With_RepetitionRule()
        {
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
                new CMMNProcessTaskProcessor(null)
            });
            var workflowInstance = CMMNWorkflowInstance.New(workflowDefinition);
            workflowEngine.Start(workflowDefinition, workflowInstance, CancellationToken.None);
            Thread.Sleep(5 * 1000);
        }

        /*
        [Fact]
        public void When_Execute_Two_Tasks_With_RepetitionRule()
        {
            var factory = BuildPlanItemProcessFactory();
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with one task")
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("increment");
                    c.SetIsBlocking(true);
                    c.AddMapping("increment", "increment");
                    c.SetRepetitionRule("activation", new CMMNExpression("language", "context.GetNumberVariable(\"increment\") &lt;= 1"));
                })
                .AddCMMNTask("2", "Second tasks", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });
                    c.SetRepetitionRule("activation", null);
                })
                .Build();
            var workflowEngine = new CMMNWorkflowEngine(factory);
            workflowEngine.Start(instance, CancellationToken.None);
            Thread.Sleep(3600 * 1000);
            string ssss = "";
        }

        [Fact]
        public void When_Execute_Tasks_With_SEntry()
        {
            var factory = BuildPlanItemProcessFactory();
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with two tasks")
                .AddCMMNTask("1", "First Task", (c) => { })
                .AddCMMNTask("2", "Second Task", (c) =>
                {
                    c.AddEntryCriterion("entry", (s) =>
                    {
                        s.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });
                })
                .Build();
            var workflowEngine = new WorkflowEngine(factory);
            workflowEngine.Start(instance, CancellationToken.None);
            Thread.Sleep(2 * 1000);

            var firstTask = (instance.Elements.First() as CMMNPlanItemDefinition).PlanItemDefinitionTask;
            var lastTask = (instance.Elements.Last() as CMMNPlanItemDefinition).PlanItemDefinitionTask;
            Assert.Equal(CMMNTaskStates.Completed, firstTask.State);
            Assert.Equal(CMMNTaskStates.Completed, lastTask.State);
        }

        [Fact]
        public void When_Execute_Task_And_Suspend()
        {
            var factory = BuildPlanItemProcessFactory();
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with two tasks")
                .AddCMMNTask("1", "First Task", (c) => { })
                .Build();

        }

        [Fact]
        public async Task When_Execute_One_ProcessTask_And_One_Simple_Task()
        {
            var factory = BuildPlanItemProcessFactory(new[] { typeof(GetGoogleHomePageTask) });
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with two tasks")
                .AddCMMNProcessTask("1", "First Task", (c) => {
                    c.SetProcessRef("PT_1");
                    c.AddMapping("html", "html");
                    c.SetIsBlocking(true);
                })
                .AddCMMNTask("2", "Second task", (c) => { })
                .AddConnection("1", "2")
                .Build();
            var workflowEngine = new WorkflowEngine(factory);
            await workflowEngine.Start(instance, CancellationToken.None);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.First().Status);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.Last().Status);
            Assert.True(instance.ExecutionContext.Variables.ContainsKey("html"));
        }

        [Fact]
        public async Task When_Execute_Two_Tasks_And_The_Second_Thrown_An_Exception()
        {
            var factory = BuildPlanItemProcessFactory(new[] { typeof(ThrowExceptionTask) });
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with two tasks")
                .AddCMMNTask("1", "First Task", (c) => { })
                .AddCMMNProcessTask("2", "Second Task", (c) => {
                    c.SetProcessRef("PT_1");
                    c.SetIsBlocking(true);
                })
                .AddConnection("1", "2")
                .Build();
            var workflowEngine = new WorkflowEngine(factory);
            await workflowEngine.Start(instance, CancellationToken.None);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.First().Status);
            Assert.Equal(ProcessFlowInstanceElementStatus.Error, instance.Elements.Last().Status);
            Assert.Equal(2, instance.ExecutionSteps.Count());
            Assert.Equal("First Task", instance.ExecutionSteps.First().ElementName);
            Assert.Equal("Second Task", instance.ExecutionSteps.Last().ElementName);
        }

        [Fact]
        public async Task When_Execute_Three_Tasks_With_OneSEntry()
        {
            var factory = BuildPlanItemProcessFactory();
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with two tasks")
                .AddCMMNTask("1", "First Task", (c) => { })
                .AddCMMNTask("2", "Second task", (c) => { })
                .AddCMMNTask("3", "Third task", (c) =>
                {

                })
                .AddConnection("1", "2")
                .AddConnection("1", "3")
                .Build();
            var workflowEngine = new WorkflowEngine(factory);
            await workflowEngine.Start(instance, CancellationToken.None);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.First().Status);
        }

        [Fact]
        public async Task When_Execute_HumanTask()
        {
            var form = new FormAggregate
            {
                Id = "form",
                Elements = new List<FormElement>
                {
                    new FormElement
                    {
                        IsRequired = true,
                        Id = "name"
                    }
                }
            };
            var jObj = new JObject
            {
                { "name", "name" }
            };
            var factory = BuildPlanItemProcessFactory();
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with one human task")
                .AddCMMNTask("1", "First task", (c) => { })
                .AddCMMNHumanTask("2", "Human task", (c) =>
                {
                    c.SetFormId("form");
                    c.SetIsBlocking(true);
                })
                .Build();
            var workflowEngine = new WorkflowEngine(factory);
            workflowEngine.Start(instance, CancellationToken.None);
            await Task.Delay(2 * 1000);
            var humanTask = instance.Elements.Last();
            instance.ConfirmForm("2", humanTask.FormInstance.Id, "form", new Dictionary<string, string>
            {
                { "name", "name" }
            });
            await Task.Delay(2 * 1000);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.First().Status);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.Last().Status);
        }

        public static ProcessFlowElementProcessorFactory BuildPlanItemProcessFactory(IEnumerable<Type> types = null)
        {
            var serviceCollection = new ServiceCollection();
            var processes = new List<ProcessAggregate>
            {
                new CaseManagementProcessAggregate
                {
                    Id = "increment",
                    AssemblyQualifiedName = typeof(IncrementTask).AssemblyQualifiedName
                }
            };
            var caseLaunchProcessCommandHandler = new CaseLaunchProcessCommandHandler(new InMemoryProcessQueryRepository(processes), new List<ICaseProcessHandler>
            {
                new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
            });
            var processors = new List<ICMMNElementProcessor>
            {
                new CMMNTaskProcessor(new ProcessorHelper()),
                new CMMNHumanTaskProcessor(new ProcessorHelper()),
                new CMMNProcessTaskProcessor(caseLaunchProcessCommandHandler, new ProcessorHelper())
            };
            if (types != null)
            {
                var processHandlers = new List<ICaseProcessHandler>
                {
                    new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
                };
                var processAggregates = new List<ProcessAggregate>();
                int i = 1;
                foreach (var type in types)
                {
                    processAggregates.Add(new CaseManagementProcessAggregate
                    {
                        Id = $"PT_{i.ToString()}",
                        AssemblyQualifiedName = type.AssemblyQualifiedName
                    });
                    i++;
                }

                var processQueryRepository = new InMemoryProcessQueryRepository(processAggregates);
                processors.Add(new CMMNProcessTaskProcessor(new CaseLaunchProcessCommandHandler(processQueryRepository, processHandlers), new ProcessorHelper()));
            }

            return new ProcessFlowElementProcessorFactory(processors);
        }
        */
    }
}
