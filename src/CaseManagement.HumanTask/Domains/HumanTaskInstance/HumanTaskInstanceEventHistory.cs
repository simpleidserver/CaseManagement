using System;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceEventHistory : ICloneable
    {
        public string EventId { get; set; }
        public DateTime EventTime { get; set; }
        public string HumanTaskIdentifier { get; set; }
        /// <summary>
        /// User (principal) that caused the state change.
        /// </summary>
        public string UserPrincipal { get; set; }
        public HumanTaskInstanceEventTypes EventType { get; set; }
        /// <summary>
        /// Even data (e.g data used in setOutput) and fault name (event was setDefault).
        /// </summary>
        public string EventData { get; set; }
        /// <summary>
        /// The actual owner before the event.
        /// </summary>
        public string StartOwner { get; set; }
        /// <summary>
        /// The actual owner after the event.
        /// </summary>
        public string EndOwner { get; set; }
        /// <summary>
        /// Task status at the end of the event.
        /// </summary>
        public HumanTaskInstanceStatus TaskStatus { get; set; }

        public object Clone()
        {
            return new HumanTaskInstanceEventHistory
            {
                EventId = EventId,
                EventTime = EventTime,
                HumanTaskIdentifier = HumanTaskIdentifier,
                UserPrincipal = UserPrincipal,
                EventType = EventType,
                EventData = EventData,
                StartOwner = StartOwner,
                EndOwner = EndOwner,
                TaskStatus = TaskStatus
            };
        }
    }
}
