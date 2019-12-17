﻿using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.CaseInstance.Processors;
using CaseManagement.CMMN.CaseInstance.Watchers;
using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.InMemory;
using CaseManagement.CMMN.Tests.Delegates;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class WorkflowEngineFixture
    {
        [Fact]
        public async Task When_Execute_Two_Simple_Tasks()
        {
            var factory = BuildPlanItemProcessFactory();
            var instance = CMMNProcessFlowInstanceBuilder.New("templateId", "Case with two tasks")
                .AddCMMNTask("1", "First Task", (c) => { })
                .AddCMMNTask("2", "Second task", (c) => { })
                .AddConnection("1", "2")
                .Build();
            var workflowEngine = new WorkflowEngine(factory);
            await workflowEngine.Start(instance, CancellationToken.None);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.First().Status);
            Assert.Equal(ProcessFlowInstanceElementStatus.Finished, instance.Elements.Last().Status);
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
            var processors = new List<IProcessFlowElementProcessor>
            {
                new CMMNTaskProcessor(new DomainEventWatcher()),
                new CMMNHumanTaskProcessor(new DomainEventWatcher())
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
                processors.Add(new CMMNProcessTaskProcessor(new CaseLaunchProcessCommandHandler(processQueryRepository, processHandlers), new DomainEventWatcher()));
            }

            return new ProcessFlowElementProcessorFactory(processors);
        }
    }
}