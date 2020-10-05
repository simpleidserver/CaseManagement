using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Complete the human task instance")]
    public class HumanTaskInstanceCompletedEvent : DomainEvent
    {
        public HumanTaskInstanceCompletedEvent(string id, string aggregateId, int version, Dictionary<string, string> outputParameters, string userPrincipal, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            OutputParameters = outputParameters;
            UserPrincipal = userPrincipal;
            UpdateDateTime = updateDateTime;
        }

        public Dictionary<string, string> OutputParameters { get; set; }
        public string UserPrincipal { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
