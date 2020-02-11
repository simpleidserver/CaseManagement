using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public class CaseWorkerTaskAggregate : BaseAggregate
    {
        public string PerformerRole { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public CaseWorkerTaskTypes TaskType { get; set; }
        public CaseWorkerTaskStatus Status { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public override object Clone()
        {
            return new CaseWorkerTaskAggregate
            {
                Id = Id,
                PerformerRole = PerformerRole,
                CasePlanInstanceId = CasePlanInstanceId,
                CasePlanElementInstanceId = CasePlanElementInstanceId,
                TaskType = TaskType,
                Status = Status
            };
        }

        public static CaseWorkerTaskAggregate New(List<DomainEvent> evts)
        {
            var result = new CaseWorkerTaskAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static CaseWorkerTaskAggregate New(string performerRole, string casePlanInstanceId, string casePlanElementInstanceId, CaseWorkerTaskTypes type)
        {
            var result = new CaseWorkerTaskAggregate();
            lock(result.DomainEvents)
            {
                var id = BuildCaseWorkerTaskIdentifier(casePlanInstanceId, casePlanElementInstanceId);
                var evt = new CaseWorkerTaskAddedEvent(Guid.NewGuid().ToString(), id, 0, performerRole, casePlanInstanceId, casePlanElementInstanceId, type, DateTime.UtcNow);
                result.Handle(evt);
                result.DomainEvents.Add(evt);
                return result;
            }
        }

        public static string BuildCaseWorkerTaskIdentifier(string casePlanInstanceId, string caseElementInstanceId)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{casePlanInstanceId}{caseElementInstanceId}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public void Confirm(IEnumerable<string> roles)
        {
            lock(DomainEvents)
            {
                if (!string.IsNullOrWhiteSpace(PerformerRole) && !roles.Contains(PerformerRole))
                {
                    throw new UnauthorizedCaseWorkerException(string.Empty, CasePlanInstanceId, CasePlanElementInstanceId);
                }

                var evt = new CaseWorkerTaskConfirmedEvent(Guid.NewGuid().ToString(), Id, Version + 1, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public override void Handle(object obj)
        {
            if(obj is CaseWorkerTaskAddedEvent)
            {
                Handle((CaseWorkerTaskAddedEvent)obj);
            }

            if (obj is CaseWorkerTaskConfirmedEvent)
            {
                Handle((CaseWorkerTaskConfirmedEvent)obj);
            }
        }

        public static string GetStreamName(string id)
        {
            return $"case-worker-task-{id}";
        }

        private void Handle(CaseWorkerTaskAddedEvent caseWorkerTaskAddedEvent)
        {
            Id = caseWorkerTaskAddedEvent.AggregateId;
            Version = caseWorkerTaskAddedEvent.Version;
            PerformerRole = caseWorkerTaskAddedEvent.PerformerRole;
            CasePlanInstanceId = caseWorkerTaskAddedEvent.CasePlanInstanceId;
            CasePlanElementInstanceId = caseWorkerTaskAddedEvent.CasePlanElementInstanceId;
            TaskType = caseWorkerTaskAddedEvent.TaskType;
            CreateDateTime = caseWorkerTaskAddedEvent.CreateDateTime;
            Status = CaseWorkerTaskStatus.Created;
        }

        private void Handle(CaseWorkerTaskConfirmedEvent caseWorkerTaskConfirmedEvent)
        {
            UpdateDateTime = caseWorkerTaskConfirmedEvent.UpdateDateTime;
            Status = CaseWorkerTaskStatus.Confirmed;
            Version = caseWorkerTaskConfirmedEvent.Version;
        }
    }
}
