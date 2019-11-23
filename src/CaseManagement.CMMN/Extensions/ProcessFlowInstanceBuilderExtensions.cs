using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.Workflow.Builders
{
    public static class ProcessFlowInstanceBuilderExtensions
    {
        public static ProcessFlowInstanceBuilder AddPlanItem(this ProcessFlowInstanceBuilder builder, string id, string name, CMMNPlanItemDefinition planItemDef, Action<CMMNPlanItemBuilder> callback)
        {
            var processTask = CMMNPlanItem.New(id, name, planItemDef);
            callback(new CMMNPlanItemBuilder(processTask));
            builder.AddElement(processTask);
            return builder;
        }
    }
}
