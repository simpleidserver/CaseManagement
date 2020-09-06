using CaseManagement.CMMN.Domains.CasePlan;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public class CasePlanAggregate : BaseAggregate
    {
        private CasePlanAggregate()
        {

        }

        public CasePlanAggregate(string id, string casePlanId, string name, string description, string caseOwner, string caseFileId, string xmlContent, IEnumerable<CasePlanRole> roles)
        {
            Id = id;
            CasePlanId = casePlanId;
            Name = name;
            Description = description;
            CaseOwner = caseOwner;
            CaseFileId = caseFileId;
            XmlContent = xmlContent;
            Roles = roles;
        }
        
        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseOwner { get; set; }
        public string CaseFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public IEnumerable<CasePlanRole> Roles { get; set; }
        public string XmlContent { get; set; }

        public static CasePlanAggregate New(List<DomainEvent> evts)
        {
            var result = new CasePlanAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static CasePlanAggregate New(string casePlanId, string name, string description, string caseOwner, string caseFileId, int caseFileVersion, string xmlContent, IEnumerable<CasePlanRole> roles)
        {
            var result = new CasePlanAggregate(BuildCasePlanIdentifier(casePlanId, caseFileVersion), casePlanId, name, description, caseOwner, caseFileId, xmlContent, roles);
            lock(result.DomainEvents)
            {
                var evt = new CasePlanAddedEvent(Guid.NewGuid().ToString(), BuildCasePlanIdentifier(casePlanId, caseFileVersion), caseFileVersion, casePlanId, name, description, caseOwner, caseFileId, DateTime.UtcNow, xmlContent, roles);
                result.Handle(evt);
                result.DomainEvents.Add(evt);
                return result;
            }
        }
        
        public override void Handle(dynamic obj)
        {
            Handle(obj);
        }

        private void Handle(CasePlanAddedEvent evt)
        {
            Id = evt.AggregateId;
            Version = evt.Version;
            CasePlanId = evt.CasePlanId;
            Name = evt.Name;
            Description = evt.Description;
            CaseOwner = evt.CaseOwner;
            CaseFileId = evt.CaseFileId;
            CreateDateTime = evt.CreateDateTime;
            XmlContent = evt.XmlContent;
            Roles = evt.Roles;
        }

        public override object Clone()
        {
            return new CasePlanAggregate(Id, CasePlanId, Name, Description, CaseOwner, CaseFileId, XmlContent, Roles.Select(_ =>
            new CasePlanRole
            {
                Id = _.Id,
                Name = _.Name
            }));
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
            return Id.GetHashCode();
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
