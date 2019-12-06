using CaseManagement.CMMN.Domains.CaseInstance.Events;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNProcessFlowInstance : ProcessFlowInstance
    {
        public static CMMNProcessFlowInstance NewCMMNProcess(string processFlowTemplateId, string processFlowName, ICollection<CMMNPlanItem> elements, ICollection<ProcessFlowConnector> connectors)
        {
            var result = new CMMNProcessFlowInstance();
            var evt = new CMMNProcessInstanceCreatedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, processFlowTemplateId, processFlowName, DateTime.UtcNow, elements, connectors);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static CMMNProcessFlowInstance NewCMMNProcess(ICollection<DomainEvent> evts)
        {
            var result = new CMMNProcessFlowInstance();
            foreach (var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public override void CallHandle(object obj)
        {
            base.CallHandle(obj);
            if (obj is CMMNProcessInstanceCreatedEvent)
            {
                Handle((CMMNProcessInstanceCreatedEvent)obj);
            }
        }

        public override string GetStreamName()
        {
            return GetCMMNStreamName(Id);
        }

        public static string GetCMMNStreamName(string id)
        {
            return $"CMMNProcessFlowInstance_{id}";
        }

        public void Handle(CMMNProcessInstanceCreatedEvent evt)
        {
            Id = evt.AggregateId;
            ProcessFlowTemplateId = evt.ProcessFlowTemplateId;
            ProcessFlowName = evt.ProcessFlowName;
            CreateDateTime = evt.CreateDateTime;
            Elements = evt.Elements.Cast<ProcessFlowInstanceElement>().ToList();
            Connectors = evt.Connectors;
            Version = evt.Version;
        }
    }
}
