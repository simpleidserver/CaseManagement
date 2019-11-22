using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.ProcessInstance.Processors;
using CaseManagement.BPMN.Tests.Delegates;
using CaseManagement.Workflow.Builders;
using CaseManagement.Workflow.Engine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CaseManagement.BPMN.Tests
{
    public class BPMNEngineFixture
    {
        [Fact]
        public async Task When_Launch_ProcessInstance()
        {
            var instance = ProcessFlowInstanceBuilder.New(new BPMNStartEvent("0", "startEvent"))
                .AddElement(new BPMNTask("1", "chooseFirstname"))
                .AddElement(new BPMNTask("2", "chooseLastname"))
                .AddElement(new BPMNEndEvent("3", "endEvent"))
                .AddConnection("0", "1")
                .AddConnection("0", "2")
                .AddConnection("1", "3")
                .AddConnection("2", "3")
                .Build();
            var secondInstance = ProcessFlowInstanceBuilder.New(new BPMNStartEvent("0", "startEvent"))
                .AddElement(new BPMNServiceTask("1", "serviceTask") { FullQualifiedName = typeof(SetFirstNameDelegate).AssemblyQualifiedName })
                .AddElement(new BPMNEndEvent("2", "endEvent"))
                .AddConnection("0", "1")
                .AddConnection("1", "2")
                .Build();

            var processors = new List<IProcessFlowElementProcessor>
            {
                new BPMNStartEventProcessor(),
                new BPMNReceiveTaskProcessor(),
                new BPMNServiceTaskProcessor(),
                new BPMNTaskProcessor(),
                new BPMNEndEventProcessor(),
            };
            var engine = new WorkflowEngine(new ProcessFlowElementProcessorFactory(processors));
            var context = new ProcessFlowInstanceExecutionContext(instance);
            await engine.Start(instance, context);
            await engine.Start(secondInstance, context);

            Assert.True(instance.IsComplete);
            Assert.True(secondInstance.IsComplete);
            Assert.Equal("simpleidserver", context.GetVariable("firstName"));
        }
    }
}