using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Notification definition is removed")]
    public class NotificationDefinitionPresentationParameterRemovedEvent : DomainEvent
    {
        public NotificationDefinitionPresentationParameterRemovedEvent(string id, string aggregateId, int version, string name, DateTime receptionDate) : base(id, aggregateId, version)
        {
            Name = name;
            ReceptionDate = receptionDate;
        }

        public string Name { get; set; }
        public DateTime ReceptionDate { get; set; }
    }
}
