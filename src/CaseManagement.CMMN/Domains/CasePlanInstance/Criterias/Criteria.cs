using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class Criteria : ICloneable
    {
        public Criteria(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public SEntry SEntry { get; set; }

        public object Clone()
        {
            return new Criteria(Name)
            {
                SEntry = SEntry == null ? null : (SEntry)SEntry.Clone()
            };
        }
    }
}
