using System;

namespace CaseManagement.HumanTask.Domains
{
    public class Escalation : ICloneable
    {
        public string Condition { get; set; }

        public object Clone()
        {
            return new Escalation
            {
                Condition = Condition
            };
        }
    }
}
