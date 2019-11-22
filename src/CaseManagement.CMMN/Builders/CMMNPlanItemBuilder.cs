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

        public CMMNPlanItemBuilder AddSEntry(string id, string name, Action<CMMNSEntryBuilder> callback)
        {
            var sEntry = new CMMNSEntry(id, name);
            callback(new CMMNSEntryBuilder(sEntry));
            _planItem.SEntries.Add(sEntry);
            return this;
        }
    }
}
