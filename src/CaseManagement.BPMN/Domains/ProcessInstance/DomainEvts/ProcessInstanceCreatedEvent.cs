using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Create process instance")]
    public class ProcessInstanceCreatedEvent : DomainEvent
    {
        public ProcessInstanceCreatedEvent(string id, string aggregateId, int version, string processFileId, string processId, DateTime createDateTime) : base(id, aggregateId, version)
        {
            ProcessFileId = processFileId;
            ProcessId = processId;
            CreateDateTime = createDateTime;
        }

        public string ProcessFileId { get; set; }
        public string ProcessId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
