using System;

namespace CaseManagement.CMMN.Domains
{
    public class Milestone : ICloneable
    {
        public Milestone(string name)
        {
        }

        public string Name { get; set; }

        public object Clone()
        {
            return new Milestone(Name);
        }
    }
}
