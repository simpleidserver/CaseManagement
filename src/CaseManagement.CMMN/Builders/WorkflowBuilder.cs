using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class WorkflowBuilder
    {
        private readonly string _processFlowTemplateId;
        private readonly string _processFlowName;
        private ICollection<CaseElementDefinition> _elements { get; set; }
        private ICollection<Criteria> _exitCriterias { get; set; }
        private string _caseOwner;

        private WorkflowBuilder(string processFlowTemplateId, string processFlowName)
        {
            _processFlowTemplateId = processFlowTemplateId;
            _processFlowName = processFlowName;
            _elements = new List<CaseElementDefinition>();
            _exitCriterias = new List<Criteria>();
        }

        public WorkflowBuilder AddCMMNPlanItem(PlanItemDefinition planItem)
        {
            _elements.Add(planItem);
            return this;
        }

        public WorkflowBuilder AddCMMNStage(string id, string name, Action<StageBuilder> callback)
        {
            var planItemDef = new StageDefinition(name);
            var stage = PlanItemDefinition.New(id, name, planItemDef);
            var builder = new StageBuilder(stage);
            callback(builder);
            _elements.Add(stage);
            return this;
        }

        public WorkflowBuilder AddCMMNTask(string id, string name, Action<TaskBuilder> callback)
        {
            var planItemDef = new CMMNTask(name);
            var processTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new TaskBuilder(processTask));
            AddCMMNPlanItem(processTask);
            return this;
        }

        public WorkflowBuilder AddDiscretionaryTask(string id, string name, Action<TaskBuilder> callback, Action<TableItemBuilder> callbackTableItem)
        {
            var planItemDef = new CMMNTask(name);
            var processTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new TaskBuilder(processTask));
            var tableItem = new TableItem();
            callbackTableItem(new TableItemBuilder(tableItem));
            processTask.TableItem = tableItem;
            AddCMMNPlanItem(processTask);
            return this;
        }

        public WorkflowBuilder AddTimerEventListener(string id, string name, Action<TimerEventListenerBuilder> callback)
        {
            var planItemDef = new TimerEventListener(name);
            var timer = PlanItemDefinition.New(id, name, planItemDef);
            callback(new TimerEventListenerBuilder(timer));
            _elements.Add(timer);
            return this;
        }

        public WorkflowBuilder AddDiscretionaryTimerEventListener(string id, string name, Action<TimerEventListenerBuilder> callback, Action<TableItemBuilder> callbackTableItem)
        {
            var planItemDef = new TimerEventListener(name);
            var timer = PlanItemDefinition.New(id, name, planItemDef);
            callback(new TimerEventListenerBuilder(timer));
            var tableItem = new TableItem();
            callbackTableItem(new TableItemBuilder(tableItem));
            timer.TableItem = tableItem;
            AddCMMNPlanItem(timer);
            return this;
        }

        public WorkflowBuilder AddCMMNMilestone(string id, string name, Action<MilestoneBuilder> callback)
        {
            var planItemDef = new Milestone(name);
            var milestone = PlanItemDefinition.New(id, name, planItemDef);
            callback(new MilestoneBuilder(milestone));
            _elements.Add(milestone);
            return this;
        }

        public WorkflowBuilder AddDiscretionaryMilestone(string id, string name, Action<MilestoneBuilder> callback, Action<TableItemBuilder> callbackTableItem)
        {
            var planItemDef = new Milestone(name);
            var milestone = PlanItemDefinition.New(id, name, planItemDef);
            callback(new MilestoneBuilder(milestone));
            var tableItem = new TableItem();
            callbackTableItem(new TableItemBuilder(tableItem));
            milestone.TableItem = tableItem;
            AddCMMNPlanItem(milestone);
            return this;
        }

        public WorkflowBuilder AddCMMNHumanTask(string id, string name, Action<HumanTaskBuilder> callback)
        {
            var planItemDef = new HumanTask(name);
            var humanTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new HumanTaskBuilder(humanTask));
            AddCMMNPlanItem(humanTask);
            return this;
        }

        public WorkflowBuilder AddDiscretionaryHumanTask(string id, string name, Action<HumanTaskBuilder> callback, Action<TableItemBuilder> callbackTableItem)
        {
            var planItemDef = new HumanTask(name);
            var humanTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new HumanTaskBuilder(humanTask));
            var tableItem = new TableItem();
            callbackTableItem(new TableItemBuilder(tableItem));
            humanTask.TableItem = tableItem;
            AddCMMNPlanItem(humanTask);
            return this;
        }

        public WorkflowBuilder AddCaseFileItem(string id, string name, Action<CaseFileItemBuilder> callback)
        {
            var caseFile = new CaseFileItemDefinition(id, name);
            callback(new CaseFileItemBuilder(caseFile));
            _elements.Add(caseFile);
            return this;
        }

        public WorkflowBuilder AddCMMNProcessTask(string id, string name, Action<ProcessTaskBuilder> callback)
        {
            var planItemDef = new ProcessTask(name);
            var processTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new ProcessTaskBuilder(processTask));
            AddCMMNPlanItem(processTask);
            return this;
        }

        public WorkflowBuilder AddDiscretionaryProcessTask(string id, string name, Action<ProcessTaskBuilder> callback, Action<TableItemBuilder> callbackTableItem)
        {
            var planItemDef = new ProcessTask(name);
            var processTask = PlanItemDefinition.New(id, name, planItemDef);
            callback(new ProcessTaskBuilder(processTask));
            var tableItem = new TableItem();
            callbackTableItem(new TableItemBuilder(tableItem));
            processTask.TableItem = tableItem;
            AddCMMNPlanItem(processTask);
            return this;
        }

        public WorkflowBuilder AddExitCriteria(string name, Action<SEntryBuilder> callback)
        {
            var sEntry = new SEntry(name);
            var exitCriteria = new Criteria(name) { SEntry = sEntry };
            callback(new SEntryBuilder(sEntry));
            _exitCriterias.Add(exitCriteria);
            return this;
        }

        public WorkflowBuilder SetCaseOwner(string caseOwner)
        {
            this._caseOwner = caseOwner;
            return this;
        }

        public CaseDefinition Build()
        {
            var result = CaseDefinition.New(_processFlowTemplateId, _processFlowName, _processFlowName, _elements, _caseOwner);
            result.ExitCriterias = _exitCriterias;
            return result;
        }

        public static WorkflowBuilder New(string id, string name)
        {
            return new WorkflowBuilder(id, name);
        }
    }
}