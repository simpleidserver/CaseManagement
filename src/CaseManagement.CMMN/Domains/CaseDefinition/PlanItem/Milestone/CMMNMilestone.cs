using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNMilestone : ICloneable
    {
        public CMMNMilestone() { }

        public CMMNMilestone(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public object Clone()
        {
            return new CMMNMilestone
            {
                Name = Name
            };
        }
    }
}
