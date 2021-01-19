using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class Escalation : ICloneable
    {
        public Escalation()
        {
            ToParts = new List<ToPart>();
        }

        public string Id { get; set; }
        public string Condition { get; set; }
        public string NotificationId { get; set; }
        /// <summary>
        /// The toParts element is used to assign values to input message of the sub-task.
        /// </summary>
        public ICollection<ToPart> ToParts { get; set; }

        public object Clone()
        {
            return new Escalation
            {
                Id = Id,
                Condition = Condition,
                NotificationId = NotificationId,
                ToParts = ToParts.Select(_ => (ToPart)_.Clone()).ToList()
            };
        }
    }
}
