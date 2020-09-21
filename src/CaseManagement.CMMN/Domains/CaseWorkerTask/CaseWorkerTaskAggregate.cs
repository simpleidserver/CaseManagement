using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public class CaseWorkerTaskAggregate : BaseAggregate
    {
        public CaseWorkerTaskAggregate()
        {
            Roles = new List<CaseWorkerTaskRole>();
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanInstanceElementId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public ICollection<CaseWorkerTaskRole> Roles { get; set; }
        

        public override object Clone()
        {
            return new CaseWorkerTaskAggregate
            {
                AggregateId = AggregateId,
                CasePlanInstanceId = CasePlanInstanceId,
                CreateDateTime = CreateDateTime,
                Version = Version,
                CasePlanInstanceElementId = CasePlanInstanceElementId,
                UpdateDateTime = UpdateDateTime,
                Roles = Roles.Select(_ => (CaseWorkerTaskRole)_.Clone()).ToList()
            };
        }

        public static CaseWorkerTaskAggregate New(CaseInstanceWorkerTaskAddedEvent evt)
        {
            var result = new CaseWorkerTaskAggregate
            {
                CasePlanInstanceId = evt.AggregateId,
                CasePlanInstanceElementId = evt.CasePlanInstanceElementId,
                CreateDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow,
                Roles = evt.CaseOwnerRole == null ? new List<CaseWorkerTaskRole>() : new List<CaseWorkerTaskRole>
                {
                    new CaseWorkerTaskRole
                    {
                        Claims = evt.CaseOwnerRole.Claims,
                        RoleId = evt.CaseOwnerRole.Id
                    }
                },
                Version = 0,
                AggregateId = BuildCaseWorkerTaskIdentifier(evt.AggregateId, evt.CasePlanInstanceElementId)
            };
            return result;
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

        #region Handle domain events

        public override void Handle(dynamic obj)
        {
            Handle(obj);
        }

        #endregion

        public static string GetStreamName(string id)
        {
            return $"case-worker-task-{id}";
        }
    }
}
