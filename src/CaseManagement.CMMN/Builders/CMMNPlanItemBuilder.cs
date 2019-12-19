using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNPlanItemBuilder
    {
        public CMMNPlanItemBuilder(CMMNPlanItemDefinition planItem)
        {
            PlanItem = planItem;
        }

        protected CMMNPlanItemDefinition PlanItem { get; private set; }

        public CMMNPlanItemBuilder AddEntryCriterion(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            var entryCriterion = new CMMNCriterion(name) { SEntry = sEntry };
            callback(new CMMNSEntryBuilder(sEntry));
            PlanItem.EntryCriterions.Add(entryCriterion);
            return this;
        }

        public CMMNPlanItemBuilder AddExitCriterion(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            var entryCriterion = new CMMNCriterion(name) { SEntry = sEntry };
            callback(new CMMNSEntryBuilder(sEntry));
            PlanItem.ExitCriterions.Add(entryCriterion);
            return this;
        }

        public CMMNPlanItemBuilder SetManualActivationRule(string name, CMMNExpression expression)
        {
            var manualActivationRule = new CMMNManualActivationRule(name, expression);
            PlanItem.SetManualActivationRule(manualActivationRule);
            return this;
        }

        public CMMNPlanItemBuilder SetRepetitionRule(string name, CMMNExpression expression)
        {
            var repetitionRule = new CMMNRepetitionRule(name, expression);
            PlanItem.SetRepetitionRule(repetitionRule);
            return this;
        }
    }
}
