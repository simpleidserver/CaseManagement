using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNWorkflowElementBuilder
    {
        public CMMNWorkflowElementBuilder(CMMNWorkflowElementDefinition workflowElementDef)
        {
            WorkflowElementDefinition = workflowElementDef;
        }

        protected CMMNWorkflowElementDefinition WorkflowElementDefinition { get; private set; }

        public CMMNWorkflowElementBuilder AddEntryCriterion(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            var entryCriterion = new CMMNCriterion(name) { SEntry = sEntry };
            callback(new CMMNSEntryBuilder(sEntry));
            WorkflowElementDefinition.EntryCriterions.Add(entryCriterion);
            return this;
        }

        public CMMNWorkflowElementBuilder AddExitCriterion(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            var entryCriterion = new CMMNCriterion(name) { SEntry = sEntry };
            callback(new CMMNSEntryBuilder(sEntry));
            WorkflowElementDefinition.ExitCriterions.Add(entryCriterion);
            return this;
        }

        public CMMNWorkflowElementBuilder SetManualActivationRule(string name, CMMNExpression expression)
        {
            var manualActivationRule = new CMMNManualActivationRule(name, expression);
            WorkflowElementDefinition.SetManualActivationRule(manualActivationRule);
            return this;
        }

        public CMMNWorkflowElementBuilder SetRepetitionRule(string name, CMMNExpression expression)
        {
            var repetitionRule = new CMMNRepetitionRule(name, expression);
            WorkflowElementDefinition.SetRepetitionRule(repetitionRule);
            return this;
        }
    }
}
