using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNWorkflowBuilder
    {
        private readonly string _processFlowTemplateId;
        private readonly string _processFlowName;
        private ICollection<CMMNWorkflowElementDefinition> _elements { get; set; }
        private ICollection<CMMNCriterion> _exitCriterias { get; set; }

        private CMMNWorkflowBuilder(string processFlowTemplateId, string processFlowName)
        {
            _processFlowTemplateId = processFlowTemplateId;
            _processFlowName = processFlowName;
            _elements = new List<CMMNWorkflowElementDefinition>();
            _exitCriterias = new List<CMMNCriterion>();
        }

        public CMMNWorkflowBuilder AddCMMNPlanItem(CMMNPlanItemDefinition planItem)
        {
            _elements.Add(planItem);
            return this;
        }

        public CMMNWorkflowBuilder AddCMMNStage(string id, string name, Action<CMMNStageBuilder> callback)
        {
            var planItemDef = new CMMNStageDefinition(name);
            var stage = CMMNPlanItemDefinition.New(id, name, planItemDef);
            var builder = new CMMNStageBuilder(stage);
            callback(builder);
            _elements.Add(stage);
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

        public CMMNWorkflowBuilder AddTimerEventListener(string id, string name, Action<CMMNTimerEventListenerBuilder> callback)
        {
            var planItemDef = new CMMNTimerEventListener(name);
            var timer = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNTimerEventListenerBuilder(timer));
            _elements.Add(timer);
            return this;
        }

        public CMMNWorkflowBuilder AddCMMNMilestone(string id, string name, Action<CMMNMilestoneBuilder> callback)
        {
            var planItemDef = new CMMNMilestone(name);
            var milestone = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNMilestoneBuilder(milestone));
            _elements.Add(milestone);
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

        public CMMNWorkflowBuilder AddCaseFileItem(string id, string name, Action<CMMNCaseFileItemBuilder> callback)
        {
            var caseFile = new CMMNCaseFileItemDefinition(id, name);
            callback(new CMMNCaseFileItemBuilder(caseFile));
            _elements.Add(caseFile);
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

        public CMMNWorkflowBuilder AddExitCriteria(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            var exitCriteria = new CMMNCriterion(name) { SEntry = sEntry };
            callback(new CMMNSEntryBuilder(sEntry));
            _exitCriterias.Add(exitCriteria);
            return this;
        }

        public CMMNWorkflowDefinition Build()
        {
            var result = CMMNWorkflowDefinition.New(_processFlowTemplateId, _processFlowName, _processFlowName, _elements);
            result.ExitCriterias = _exitCriterias;
            return result;
        }

        public static CMMNWorkflowBuilder New(string id, string name)
        {
            return new CMMNWorkflowBuilder(id, name);
        }
    }
}