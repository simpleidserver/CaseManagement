using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanAggregate : BaseAggregate
    {
        public CasePlanAggregate()
        {

        }

        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseFileId { get; set; }
        public string XmlContent { get; set; }
        public int NbInstances { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<CasePlanRole> Roles { get; set; }
        public ICollection<CasePlanFileItem> Files { get; set; }

        public static CasePlanAggregate New(List<DomainEvent> evts)
        {
            var result = new CasePlanAggregate();
            foreach (var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static CasePlanAggregate New(string casePlanId, string name, string description, string caseFileId, int caseFileVersion, string xmlContent, ICollection<CasePlanRole> roles, ICollection<CasePlanFileItem> files)
        {
            var result = new CasePlanAggregate();
            var evt = new CasePlanAddedEvent(Guid.NewGuid().ToString(), BuildCasePlanIdentifier(casePlanId, caseFileVersion), caseFileVersion, casePlanId, name, description, caseFileId, DateTime.UtcNow, xmlContent, roles, files);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public void IncrementInstance()
        {
            NbInstances++;
        }

        public override void Handle(dynamic obj)
        {
            Handle(obj);
        }

        private void Handle(CasePlanAddedEvent evt)
        {
            AggregateId = evt.AggregateId;
            Version = evt.Version;
            CasePlanId = evt.CasePlanId;
            Name = evt.Name;
            Description = evt.Description;
            CaseFileId = evt.CaseFileId;
            CreateDateTime = evt.CreateDateTime;
            XmlContent = evt.XmlContent;
            Roles = evt.Roles.ToList();
            Files = evt.Files;
        }

        public override object Clone()
        {
            return new CasePlanAggregate
            {
                AggregateId = AggregateId,
                CasePlanId = CasePlanId,
                Name = Name,
                Description = Description,
                NbInstances = NbInstances,
                CaseFileId = CaseFileId,
                XmlContent = XmlContent,
                Roles = Roles.Select(_ => (CasePlanRole)_.Clone()).ToList(),
                Files = Files.Select(_ => (CasePlanFileItem)_.Clone()).ToList(),
                CreateDateTime = CreateDateTime,
                Version = Version
            };
        }

        public override bool Equals(object obj)
        {
            var target = obj as CasePlanAggregate;
            if (target == null)
            {
                return false;
            }

            return target.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return AggregateId.GetHashCode();
        }

        public static string BuildCasePlanIdentifier(string casePlanId, int version)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{casePlanId}{version}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
