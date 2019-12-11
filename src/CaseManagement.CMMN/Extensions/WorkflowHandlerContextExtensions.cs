using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Engine;

namespace CaseManagement.CMMN.Extensions
{
    public static class WorkflowHandlerContextExtensions
    {
        public static CMMNPlanItem GetCMMNPlanItem(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.CurrentElement as CMMNPlanItem;
        }

        public static CMMNCaseFileItem GetCMMNCaseFileItem(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.CurrentElement as CMMNCaseFileItem;
        }

        public static CMMNTask GetCMMNTask(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinitionTask;
        }

        public static CMMNProcessTask GetCMMNProcessTask(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinitionProcessTask;
        }

        public static CMMNHumanTask GetCMMNHumanTask(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinitionHumanTask;
        }

        public static CMMNTimerEventListener GetCMMNTimerEventListener(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinitionTimerEventListener;
        }
    }
}
