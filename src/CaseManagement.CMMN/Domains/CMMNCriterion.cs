using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNCriterion : ICloneable
    {
        public CMMNCriterion(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public CMMNSEntry SEntry { get; set; }

        public object Clone()
        {
            return new CMMNCriterion(Name)
            {
                SEntry = SEntry == null ? null : (CMMNSEntry)SEntry.Clone()
            };
        }
    }
}
