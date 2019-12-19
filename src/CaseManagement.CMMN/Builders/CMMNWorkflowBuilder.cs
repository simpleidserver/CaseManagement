using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNWorkflowBuilder
    {
        private string _processFlowTemplateId;
        private string _processFlowName;
        private ICollection<CMMNPlanItemDefinition> _planItems { get; set; }

        private CMMNWorkflowBuilder(string processFlowTemplateId, string processFlowName)
        {
            _processFlowTemplateId = processFlowTemplateId;
            _processFlowName = processFlowName;
            _planItems = new List<CMMNPlanItemDefinition>();
        }

        public CMMNWorkflowBuilder AddCMMNPlanItem(CMMNPlanItemDefinition planItem)
        {
            _planItems.Add(planItem);
            return this;
        }

        public CMMNWorkflowBuilder AddCMMNTask(string id, string name, Action<CMMNTaskBuilder> callback)
        {
            var planItemDef = new CMMNTask(name);
            var processTask = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNTaskBuilder(processTask));
            AddCMMNPlanItem(processTask);
            return this;
        }

        public CMMNWorkflowBuilder AddCMMNHumanTask(string id, string name, Action<CMMNHumanTaskBuilder> callback)
        {
            var planItemDef = new CMMNHumanTask(name);
            var humanTask = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNHumanTaskBuilder(humanTask));
            AddCMMNPlanItem(humanTask);
            return this;
        }

        public CMMNWorkflowBuilder AddCMMNProcessTask(string id, string name, Action<CMMNProcessTaskBuilder> callback)
        {
            var planItemDef = new CMMNProcessTask(name);
            var processTask = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNProcessTaskBuilder(processTask));
            AddCMMNPlanItem(processTask);
            return this;
        }

        public CMMNWorkflowDefinition Build()
        {
            return CMMNWorkflowDefinition.New(_processFlowTemplateId, _processFlowTemplateId, "", _planItems);
        }

        public static CMMNWorkflowBuilder New(string id, string name)
        {
            return new CMMNWorkflowBuilder(id, name);
        }
    }
}