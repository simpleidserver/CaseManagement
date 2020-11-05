using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public enum DeadlineUsages
    {
        START = 0,
        COMPLETION = 1
    }

    public class HumanTaskDefinitionDeadLine : ICloneable
    {
        public HumanTaskDefinitionDeadLine()
        {
            Escalations = new List<Escalation>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string For { get; set; }
        public string Until { get; set; }
        public DeadlineUsages Usage { get; set; }
        /// <summary>
        /// If a status is not reached within a certain time then an escalation action can be triggered.
        /// Escalations are triggered if :
        /// The associated point in time is reached, or duration has elasped and the associated condition evaluates to true.
        /// </summary>
        public ICollection<Escalation> Escalations { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionDeadLine
            {
                Id = Id,
                Name = Name,
                For = For,
                Until = Until,
                Usage = Usage,
                Escalations = Escalations.Select(_ => (Escalation)_.Clone()).ToList()
            };
        }
    }
}
