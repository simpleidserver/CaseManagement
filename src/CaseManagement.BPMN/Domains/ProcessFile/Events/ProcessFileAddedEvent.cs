﻿using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Add process definition")]
    [Serializable]
    public class ProcessFileAddedEvent : DomainEvent
    {
        public ProcessFileAddedEvent(string id, string aggregateId, int version, string fileId, string name, string description, DateTime createDateTime, string payload) : base(id, aggregateId, version)
        {
            FileId = fileId;
            Name = name;
            Description = description;
            CreateDateTime = createDateTime;
            Payload = payload;
        }

        public string FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Payload { get; set; }
    }
}
