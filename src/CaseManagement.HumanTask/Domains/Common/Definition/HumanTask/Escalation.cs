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
            Notification = new NotificationDefinition();
        }

        public string Id { get; set; }
        public string Condition { get; set; }
        public NotificationDefinition Notification { get; set; }
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
                Notification = Notification,
                ToParts = ToParts.Select(_ => (ToPart)_.Clone()).ToList()
            };
        }
    }
}
