using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.Workflow.Builders
{
    public static class ProcessFlowInstanceBuilderExtensions
    {
        public static ProcessFlowInstanceBuilder AddCMMNTask(this ProcessFlowInstanceBuilder builder, string id, string name, Action<CMMNTaskBuilder> callback)
        {
            var planItemDef = new CMMNTask(name);
            var processTask = CMMNPlanItem.New(id, name, planItemDef);
            callback(new CMMNTaskBuilder(processTask));
            builder.AddElement(processTask);
            return builder;
        }

        public static ProcessFlowInstanceBuilder AddCMMNHumanTask(this ProcessFlowInstanceBuilder builder, string id, string name, Action<CMMNHumanTaskBuilder> callback)
        {
            var planItemDef = new CMMNHumanTask(name);
            var humanTask = CMMNPlanItem.New(id, name, planItemDef);
            callback(new CMMNHumanTaskBuilder(humanTask));
            builder.AddElement(humanTask);
            return builder;
        }

        public static ProcessFlowInstanceBuilder AddCMMNProcessTask(this ProcessFlowInstanceBuilder builder, string id, string name, Action<CMMNProcessTaskBuilder> callback)
        {
            var planItemDef = new CMMNProcessTask(name);
            var processTask = CMMNPlanItem.New(id, name, planItemDef);
            callback(new CMMNProcessTaskBuilder(processTask));
            builder.AddElement(processTask);
            return builder;
        }

        public static ProcessFlowInstanceBuilder AddPlanItem(this ProcessFlowInstanceBuilder builder, CMMNPlanItem planItem)
        {
            builder.AddElement(planItem);
            return builder;
        }
    }
}
