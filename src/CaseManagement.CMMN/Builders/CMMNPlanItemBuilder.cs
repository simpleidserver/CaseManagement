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

        public CMMNPlanItemBuilder AddSEntry(string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(name);
            callback(new CMMNSEntryBuilder(sEntry));
            _planItem.SEntries.Add(sEntry);
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
