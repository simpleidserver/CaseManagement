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
        private readonly IProcessorHelper _processorHelper;

        public CMMNMilestoneProcessor(IProcessorHelper processorHelper)
        {
            _processorHelper = processorHelper;
        }

        public string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.Milestone).ToLowerInvariant();

        public async Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var milestone = cmmnPlanItem.PlanItemMilestone;
            if (cmmnPlanItem.Status == null)
            {
                context.Start(token);
                pf.CreatePlanItem(cmmnPlanItem);
            }
            else
            {
                var result = _processorHelper.HandleRepetitionRule(cmmnPlanItem, pf);
                if (result != null && result == RepetitionRuleResultTypes.Complete)
                {
                    context.Complete();
                    return;
                }
            }

            if (milestone.State == CMMNMilestoneStates.Available)
            {
                if (cmmnPlanItem.EntryCriterions.Any() && cmmnPlanItem.EntryCriterions.All(s => !BaseCMMNTaskProcessor.CheckCriterion(s, pf, cmmnPlanItem.Version)))
                {
                    return;
                }
            }

            pf.OccurPlanItem(cmmnPlanItem);
            await context.ExecuteNext(token);
        }
    }
}
