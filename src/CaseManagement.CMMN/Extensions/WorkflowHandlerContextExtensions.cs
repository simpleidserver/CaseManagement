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

        public static CMMNTask GetCMMNTask(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinition as CMMNTask;
        }

        public static CMMNProcessTask GetCMMNProcessTask(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinition as CMMNProcessTask;
        }

        public static CMMNHumanTask GetCMMNHumanTask(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinition as CMMNHumanTask;
        }

        public static CMMNTimerEventListener GetCMMNTimerEventListener(this WorkflowHandlerContext workflowHandlerContext)
        {
            return workflowHandlerContext.GetCMMNPlanItem().PlanItemDefinition as CMMNTimerEventListener;
        }
    }
}
