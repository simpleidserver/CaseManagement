using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains.HumanTaskDef.Events
{
    [DebuggerDisplay("Presentation parameter '{Name}' is removed")]
    public class HumanTaskPresentationParameterRemovedEvent : DomainEvent
    {
        public HumanTaskPresentationParameterRemovedEvent(string id, string aggregateId, int version, string name, DateTime receptionDate) : base(id, aggregateId, version)
        {
            Name = name;
            ReceptionDate = receptionDate;
        }

        public string Name { get; set; }
        public DateTime ReceptionDate { get; set; }
    }
}
