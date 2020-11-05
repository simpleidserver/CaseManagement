using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceDeadLine : ICloneable
    {
        public HumanTaskInstanceDeadLine()
        {
            Escalations = new List<Escalation>();
        }

        public string Name { get; set; }
        public DeadlineUsages Usage { get; set; }
        public DateTime EndDateTime { get; set; }
        public ICollection<Escalation> Escalations { get; set; }

        public object Clone()
        {
            return new HumanTaskInstanceDeadLine
            {
                Name = Name,
                Usage = Usage,
                EndDateTime = EndDateTime,
                Escalations = Escalations.Select(_ => (Escalation)_.Clone()).ToList()
            };
        }
    }
}
