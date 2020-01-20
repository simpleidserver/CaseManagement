using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class SEntryBuilder
    {
        private SEntry _sEntry;

        public SEntryBuilder(SEntry sEntry)
        {
            _sEntry = sEntry;
        }

        public SEntryBuilder AddOnPart(PlanItemOnPart itemPart)
        {
            _sEntry.PlanItemOnParts.Add(itemPart);
            return this;
        }

        public SEntryBuilder AddOnPart(CaseFileItemOnPart itemPart)
        {
            _sEntry.FileItemOnParts.Add(itemPart);
            return this;
        }

        public SEntryBuilder SetIfPart(string condition)
        {
            _sEntry.IfPart = new IfPart { Condition = condition };
            return this;
        }

        public SEntry Build()
        {
            return _sEntry;
        }
    }
}
