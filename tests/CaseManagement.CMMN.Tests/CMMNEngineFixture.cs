using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.ProcessInstance.Processors;
using CaseManagement.Workflow.Builders;
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
            var instance = ProcessFlowInstanceBuilder.New(new CMMNPlanItem("0", "Assign reviewer"))
                .AddProcessTask("1", "Review reviewer", (c) =>
                {
                    c.AddSEntry("2", "sEntry", (ca) =>
                    {
                        ca.AddOnPart(new CMMNPlanItemOnPart { SourceRef = "0", StandardEvent = CMMNPlanItemTransitions.Complete });
                    });
                })
                .AddConnection("0", "1")
                .Build();
            var processors = new List<IProcessFlowElementProcessor>
            {
                new CMMNPlanItemProcessor(),
                new CMMNProcessTaskProcessor()
            };
            var engine = new WorkflowEngine(new ProcessFlowElementProcessorFactory(processors));
            var context = new ProcessFlowInstanceExecutionContext(instance);
            await engine.Start(instance, context);
            var runningElts = instance.GetRunningElements();
            var planItem = runningElts.First(r => r.Id == "0") as CMMNPlanItem;
            planItem.Transition = CMMNPlanItemTransitions.Complete;
            await engine.Start(instance, context);
        }
    }
}
