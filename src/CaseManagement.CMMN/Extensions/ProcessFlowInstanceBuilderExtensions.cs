using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.Workflow.Builders
{
    public static class ProcessFlowInstanceBuilderExtensions
    {
        public static ProcessFlowInstanceBuilder AddProcessTask(this ProcessFlowInstanceBuilder builder, string id, string name, Action<CMMNProcessTaskBuilder> callback)
        {
            var processTask = new CMMNProcessTask(id, name);
            callback(new CMMNProcessTaskBuilder(processTask));
            builder.AddElement(processTask);
            return builder;
        }
    }
}
