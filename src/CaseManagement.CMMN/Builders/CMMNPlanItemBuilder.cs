using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNPlanItemBuilder
    {
        private readonly CMMNPlanItem _planItem;

        public CMMNPlanItemBuilder(CMMNPlanItem planItem)
        {
            _planItem = planItem;
        }

        public CMMNPlanItemBuilder AddEntryCriterion(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            var entryCriterion = new CMMNCriterion(name) { SEntry = sEntry };
            callback(new CMMNSEntryBuilder(sEntry));
            _planItem.EntryCriterions.Add(entryCriterion);
            return this;
        }

        public CMMNPlanItemBuilder AddExitCriterion(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            var entryCriterion = new CMMNCriterion(name) { SEntry = sEntry };
            callback(new CMMNSEntryBuilder(sEntry));
            _planItem.ExitCriterions.Add(entryCriterion);
            return this;
        }

        public CMMNPlanItemBuilder SetManualActivationRule(string name, CMMNExpression expression)
        {
            var manualActivationRule = new CMMNManualActivationRule(name, expression);
            _planItem.PlanItemControl = manualActivationRule;
            return this;
        }

        public CMMNPlanItemBuilder SetProcessTaskDefinition(string name)
        {
            _planItem.PlanItemDefinition = new CMMNProcessTask(name);
            return this;
        }
    }
}
