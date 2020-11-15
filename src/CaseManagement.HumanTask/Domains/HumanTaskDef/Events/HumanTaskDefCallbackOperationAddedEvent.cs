using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Add callback operation")]
    public class HumanTaskDefCallbackOperationAddedEvent : DomainEvent
    {
        public HumanTaskDefCallbackOperationAddedEvent(string id, string aggregateId, int version, CallbackOperation callbackOperation, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            CallbackOperation = callbackOperation;
            UpdateDateTime = updateDateTime;
        }

        public CallbackOperation CallbackOperation { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
