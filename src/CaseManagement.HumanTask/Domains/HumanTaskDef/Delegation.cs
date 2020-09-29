using System;

namespace CaseManagement.HumanTask.Domains
{
    public class Delegation : ICloneable
    {
        public DelegationTypes Type { get; set; }

        public object Clone()
        {
            return new Delegation
            {
                Type = Type
            };
        }
    }
}
