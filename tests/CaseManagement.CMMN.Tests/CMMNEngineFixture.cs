using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.ProcessInstance.Processors;
using CaseManagement.Workflow.Builders;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System.Collections.Generic;
using System.Linq;
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
                    c.AddEntryCriterion("sEntry", (ca) =>
                    {
                        ca.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "0", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });                    
                })
                .AddPlanItem("2", "Manual task", new CMMNProcessTask("Manual task"), (c) =>
                {
                    c.AddEntryCriterion("sEntry", (ca) =>
                    {
                        ca.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });
                    c.SetManualActivationRule("Manual activation", new CMMNExpression("language", "true"));
                })
                .AddPlanItem("3", "Terminate task", new CMMNProcessTask("Terminate task"), (c) =>
                {
                    c.AddExitCriterion("sExit", (ca) =>
                    {
                        ca.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "1", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });
                })
                .AddPlanItem("4", "Finish", new CMMNProcessTask("Finish"), (c) => { })
                .AddPlanItem("5", "Second Finish", new CMMNProcessTask("Second Finish"), (c) => { })
                .AddConnection("0", "1")
                .AddConnection("1", "2")
                .AddConnection("1", "3")
                .AddConnection("2", "4")
                .AddConnection("3", "5")
                .Build();
            var processors = new List<IProcessFlowElementProcessor>
            {
                new CMMNPlanItemProcessor()
            };
            var engine = new WorkflowEngine(new ProcessFlowElementProcessorFactory(processors));
            var context = new ProcessFlowInstanceExecutionContext(instance);
            await engine.Start(instance, context);

            instance.ManuallyStartPlanItem("2");

            await engine.Start(instance, context);

            Assert.True(instance.IsComplete);
            var firstPlanItem = (CMMNProcessTask)(instance.Elements.First(e => e.Id == "0") as CMMNPlanItem).PlanItemDefinition;
            var secondPlanItem = (CMMNProcessTask)(instance.Elements.First(e => e.Id == "1") as CMMNPlanItem).PlanItemDefinition;
            var thirdPlanItem = (CMMNProcessTask)(instance.Elements.First(e => e.Id == "2") as CMMNPlanItem).PlanItemDefinition;
            var fourthPlanItem = (CMMNProcessTask)(instance.Elements.First(e => e.Id == "3") as CMMNPlanItem).PlanItemDefinition;
            var fifthPlanItem = (CMMNProcessTask)(instance.Elements.First(e => e.Id == "4") as CMMNPlanItem).PlanItemDefinition;
            Assert.Equal(CMMNTaskStates.Completed, firstPlanItem.State);
            Assert.Equal(CMMNTaskStates.Completed, secondPlanItem.State);
            Assert.Equal(CMMNTaskStates.Completed, thirdPlanItem.State);
            Assert.Equal(CMMNTaskStates.Terminated, fourthPlanItem.State);
            Assert.Equal(CMMNTaskStates.Completed, fifthPlanItem.State);
        }
    }
}
