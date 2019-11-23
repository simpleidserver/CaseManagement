using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.ProcessInstance.Processors;
using CaseManagement.Workflow.Builders;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class CMMNEngineFixture
    {
        [Fact]
        public async Task When_Launch_CaseInstance()
        {
            var instance = ProcessFlowInstanceBuilder.New(CMMNPlanItem.New("0", "Assign reviewer", new CMMNProcessTask("Assign reviewer")))
                .AddPlanItem("1", "Review reviewer", new CMMNProcessTask("Review reviewer"), (c) =>
                {
                    c.AddSEntry("sEntry", (ca) =>
                    {
                        ca.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "0", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });                    
                })
                .AddPlanItem("2", "Manual task", new CMMNProcessTask("Manual task"), (c) =>
                {
                    c.AddSEntry("sEntry", (ca) =>
                    {
                        ca.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });
                    c.SetManualActivationRule("Manual activation", new CMMNExpression("language", "true"));
                })
                .AddPlanItem("3", "Finish", new CMMNProcessTask("Finish"), (c) => { })
                .AddConnection("0", "1")
                .AddConnection("1", "2")
                .AddConnection("2", "3")
                .Build();
            var processors = new List<IProcessFlowElementProcessor>
            {
                new CMMNPlanItemProcessor()
            };
            var engine = new WorkflowEngine(new ProcessFlowElementProcessorFactory(processors));
            var context = new ProcessFlowInstanceExecutionContext(instance);
            await engine.Start(instance, context);
            Assert.False(instance.IsComplete);

            instance.ManuallyStartPlanItem("2");

            await engine.Start(instance, context);

            Assert.True(instance.IsComplete);

            // instance.DisablePlanItem("0");
            // await engine.Start(instance, context);
            // 
            // var firstPlanItem = instance.Elements.First(e => e.Id == "1");
            // var secondPlanItem = instance.Elements.First(e => e.Id == "3");
            // var thirdPlanItem = instance.Elements.First(e => e.Id == "5");
            // Assert.True(instance.IsComplete);
            // Assert.Equal(ProcessFlowInstanceElementStatus.Blocked, firstPlanItem.Status);
            // Assert.Equal(ProcessFlowInstanceElementStatus.Finished, secondPlanItem.Status);
            // Assert.Equal(ProcessFlowInstanceElementStatus.Finished, thirdPlanItem.Status);
        }
    }
}
