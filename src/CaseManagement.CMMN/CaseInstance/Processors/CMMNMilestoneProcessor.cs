using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    /*
    public class CMMNMilestoneProcessor : IProcessFlowElementProcessor
    {
        private readonly IProcessorHelper _processorHelper;

        public CMMNMilestoneProcessor(IProcessorHelper processorHelper)
        {
            _processorHelper = processorHelper;
        }

        public string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.Milestone).ToLowerInvariant();

        public Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var milestone = cmmnPlanItem.PlanItemMilestone;
            if (cmmnPlanItem.Status == null)
            {
                pf.CreatePlanItem(cmmnPlanItem);
            }
            else
            {
                var result = _processorHelper.HandleRepetitionRule(cmmnPlanItem, pf);
                if (result != null && result == RepetitionRuleResultTypes.Complete)
                {
                    return Task.CompletedTask;
                }
            }

            if (milestone.State == CMMNMilestoneStates.Available)
            {
            }

            pf.OccurPlanItem(cmmnPlanItem);
            return Task.CompletedTask;
        }
    }
    */
}
