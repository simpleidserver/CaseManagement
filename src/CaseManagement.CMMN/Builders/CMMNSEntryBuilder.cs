using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNSEntryBuilder
    {
        private CMMNSEntry _sEntry;

        public CMMNSEntryBuilder(CMMNSEntry sEntry)
        {
            _sEntry = sEntry;
        }

        public CMMNSEntryBuilder AddOnPart(CMMNPlanItemOnPart itemPart)
        {
            _sEntry.OnParts.Add(itemPart);
            return this;
        }

        public CMMNSEntryBuilder SetIfPart(string condition)
        {
            _sEntry.IfPart = new CMMNIfPart { Condition = condition };
            return this;
        }

        public CMMNSEntry Build()
        {
            return _sEntry;
        }
    }
}
