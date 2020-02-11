using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class StageBuilder : WorkflowElementBuilder
    {
        public StageBuilder(PlanItemDefinition stageDefinition) : base(stageDefinition)
        {
        }

        public StageBuilder AddElement(CasePlanElement element)
        {
            var stage = (WorkflowElementDefinition as PlanItemDefinition).Stage;
            stage.Elements.Add(element);
            return this;
        }

        public StageBuilder AddCMMNStage(string id, string name, Action<StageBuilder> callback)
        {
            var planItemDef = new StageDefinition(name);
            var stage = PlanItemDefinition.New(id, name, planItemDef);
            var builder = new StageBuilder(stage);
            callback(builder);
            AddElement(stage);
            return this;
        }

        public StageBuilder AddCMMNTask(string id, string name, Action<TaskBuilder> callback)
        {
            var planItemDef = new CMMNTask(name);
            var processTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new TaskBuilder(processTask));
            AddElement(processTask);
            return this;
        }

        public StageBuilder AddCMMNHumanTask(string id, string name, Action<HumanTaskBuilder> callback)
        {
            var planItemDef = new HumanTask(name);
            var humanTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new HumanTaskBuilder(humanTask));
            AddElement(humanTask);
            return this;
        }

        public StageBuilder AddCMMNProcessTask(string id, string name, Action<ProcessTaskBuilder> callback)
        {
            var planItemDef = new ProcessTask(name);
            var processTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new ProcessTaskBuilder(processTask));
            AddElement(processTask);
            return this;
        }
    }
}
