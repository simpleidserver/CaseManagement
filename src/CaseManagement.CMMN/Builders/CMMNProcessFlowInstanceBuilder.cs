using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Builders;
using CaseManagement.Workflow.Domains;
using System;
using System.Linq;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNProcessFlowInstanceBuilder : ProcessFlowInstanceBuilder
    {
        private CMMNProcessFlowInstanceBuilder(string processFlowTemplateId, string processFlowName) : base(processFlowTemplateId, processFlowName) { }

        public override ProcessFlowInstanceBuilder AddElement(ProcessFlowInstanceElement node)
        {
            return base.AddElement(node);
        }

        public CMMNProcessFlowInstanceBuilder AddCMMNTask(string id, string name, Action<CMMNTaskBuilder> callback)
        {
            var planItemDef = new CMMNTask(name);
            var processTask = CMMNPlanItem.New(id, name, planItemDef);
            callback(new CMMNTaskBuilder(processTask));
            AddElement(processTask);
            return this;
        }

        public CMMNProcessFlowInstanceBuilder AddCMMNHumanTask(string id, string name, Action<CMMNHumanTaskBuilder> callback)
        {
            var planItemDef = new CMMNHumanTask(name);
            var humanTask = CMMNPlanItem.New(id, name, planItemDef);
            callback(new CMMNHumanTaskBuilder(humanTask));
            AddElement(humanTask);
            return this;
        }

        public CMMNProcessFlowInstanceBuilder AddCMMNProcessTask(string id, string name, Action<CMMNProcessTaskBuilder> callback)
        {
            var planItemDef = new CMMNProcessTask(name);
            var processTask = CMMNPlanItem.New(id, name, planItemDef);
            callback(new CMMNProcessTaskBuilder(processTask));
            AddElement(processTask);
            return this;
        }

        public CMMNProcessFlowInstanceBuilder AddPlanItem(CMMNPlanItem planItem)
        {
            AddElement(planItem);
            return this;
        }

        public CMMNProcessFlowInstanceBuilder AddCaseFileItem(CMMNCaseFileItem fileItem)
        {
            AddElement(fileItem);
            return this;
        }

        public override ProcessFlowInstance Build()
        {
            return CMMNProcessFlowInstance.NewCMMNProcess(ProcessFlowTemplateId, ProcessFlowName, Elements.Where(e => e is CMMNPlanItem).Cast<CMMNPlanItem>().ToList(), Elements.Where(e => e is CMMNCaseFileItem).Cast<CMMNCaseFileItem>().ToList(), Connectors);
        }

        public static new CMMNProcessFlowInstanceBuilder New(string processFlowTemplateId, string processFlowName)
        {
            return new CMMNProcessFlowInstanceBuilder(processFlowTemplateId, processFlowName);
        }
    }
}