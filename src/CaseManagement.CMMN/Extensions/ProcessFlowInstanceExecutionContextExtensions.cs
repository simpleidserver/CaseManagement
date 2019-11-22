using CaseManagement.CMMN.Domains;

namespace CaseManagement.Workflow.Engine
{
    public static class ProcessFlowInstanceExecutionContextExtensions
    {
        public static CMMNPlanItem GetPlanItem(this ProcessFlowInstanceExecutionContext ctx, string id)
        {
            return ctx.GetElement(id) as CMMNPlanItem;
        }
    }
}
