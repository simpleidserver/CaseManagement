using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNMilestoneProcessor : IProcessFlowElementProcessor
    {
        public string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.Milestone).ToLowerInvariant();

        public async Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            context.Start();
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var milestone = cmmnPlanItem.PlanItemMilestone;
            if (milestone.State == CMMNMilestoneStates.Available)
            {
                if (cmmnPlanItem.EntryCriterions.Any() && cmmnPlanItem.EntryCriterions.All(s => !BaseCMMNTaskProcessor.CheckCriterion(s, pf)))
                {
                    pf.BlockElement(cmmnPlanItem);
                    return;
                }
            }

            pf.OccurPlanItem(cmmnPlanItem);
            await context.Complete(token);
        }
    }
}
