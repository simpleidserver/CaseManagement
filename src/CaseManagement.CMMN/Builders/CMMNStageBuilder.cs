using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNStageBuilder : CMMNWorkflowElementBuilder
    {
        public CMMNStageBuilder(CMMNPlanItemDefinition stageDefinition) : base(stageDefinition)
        {
        }

        public CMMNStageBuilder AddElement(CMMNWorkflowElementDefinition element)
        {
            var stage = (WorkflowElementDefinition as CMMNPlanItemDefinition).Stage;
            stage.Elements.Add(element);
            return this;
        }

        public CMMNStageBuilder AddCMMNStage(string id, string name, Action<CMMNStageBuilder> callback)
        {
            var planItemDef = new CMMNStageDefinition(name);
            var stage = CMMNPlanItemDefinition.New(id, name, planItemDef);
            var builder = new CMMNStageBuilder(stage);
            callback(builder);
            AddElement(stage);
            return this;
        }

        public CMMNStageBuilder AddCMMNTask(string id, string name, Action<CMMNTaskBuilder> callback)
        {
            var planItemDef = new CMMNTask(name);
            var processTask = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNTaskBuilder(processTask));
            AddElement(processTask);
            return this;
        }

        public CMMNStageBuilder AddCMMNHumanTask(string id, string name, Action<CMMNHumanTaskBuilder> callback)
        {
            var planItemDef = new CMMNHumanTask(name);
            var humanTask = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNHumanTaskBuilder(humanTask));
            AddElement(humanTask);
            return this;
        }

        public CMMNStageBuilder AddCMMNProcessTask(string id, string name, Action<CMMNProcessTaskBuilder> callback)
        {
            var planItemDef = new CMMNProcessTask(name);
            var processTask = CMMNPlanItemDefinition.New(id, name, planItemDef);
            callback(new CMMNProcessTaskBuilder(processTask));
            AddElement(processTask);
            return this;
        }
    }
}
