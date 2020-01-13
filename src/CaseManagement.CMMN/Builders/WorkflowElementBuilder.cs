using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.Builders
{
    public class WorkflowElementBuilder
    {
        public WorkflowElementBuilder(CaseElementDefinition workflowElementDef)
        {
            WorkflowElementDefinition = workflowElementDef;
        }

        protected CaseElementDefinition WorkflowElementDefinition { get; private set; }

        public WorkflowElementBuilder AddEntryCriterion(string name, Action<SEntryBuilder> callback)
        {
            var sEntry = new SEntry(name);
            var entryCriterion = new Criteria(name) { SEntry = sEntry };
            callback(new SEntryBuilder(sEntry));
            WorkflowElementDefinition.EntryCriterions.Add(entryCriterion);
            return this;
        }

        public WorkflowElementBuilder AddExitCriterion(string name, Action<SEntryBuilder> callback)
        {
            var sEntry = new SEntry(name);
            var entryCriterion = new Criteria(name) { SEntry = sEntry };
            callback(new SEntryBuilder(sEntry));
            WorkflowElementDefinition.ExitCriterions.Add(entryCriterion);
            return this;
        }

        public WorkflowElementBuilder SetManualActivationRule(string name, CMMNExpression expression)
        {
            var manualActivationRule = new ManualActivationRule(name, expression);
            WorkflowElementDefinition.SetManualActivationRule(manualActivationRule);
            return this;
        }

        public WorkflowElementBuilder SetRepetitionRule(string name, CMMNExpression expression)
        {
            var repetitionRule = new RepetitionRule(name, expression);
            WorkflowElementDefinition.SetRepetitionRule(repetitionRule);
            return this;
        }
    }
}
