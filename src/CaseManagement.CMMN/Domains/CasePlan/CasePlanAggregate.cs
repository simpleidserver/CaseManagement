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

        public CasePlanAggregate(string id, string casePlanId, string name, string description, string caseOwner, string caseFileId)
        {
            Id = id;
            CasePlanId = casePlanId;
            Name = name;
            Description = description;
            CaseOwner = caseOwner;
            CaseFileId = caseFileId;
            ExitCriterias = new List<Criteria>();
            Elements = new List<CasePlanElement>();
        }
        
        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseOwner { get; set; }
        public string CaseFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<Criteria> ExitCriterias { get; set; }
        public ICollection<CasePlanElement> Elements { get; set; }

        public CasePlanElement GetElement(string id)
        {
            return GetElement(Elements, id);
        }

        public static CasePlanAggregate New(List<DomainEvent> evts)
        {
            var result = new CasePlanAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static CasePlanAggregate New(string casePlanId, string name, string description, string caseOwner, string caseFileId, int caseFileVersion, ICollection<Criteria> exitCriterias, ICollection<CasePlanElement> elements)
        {
            var result = new CasePlanAggregate(BuildCasePlanIdentifier(casePlanId, caseFileVersion), casePlanId, name, description, caseOwner, caseFileId);
            lock(result.DomainEvents)
            {
                var evt = new CasePlanAddedEvent(Guid.NewGuid().ToString(), BuildCasePlanIdentifier(casePlanId, caseFileVersion), caseFileVersion, casePlanId, name, description, caseOwner, caseFileId, DateTime.UtcNow, exitCriterias, elements);
                result.Handle(evt);
                result.DomainEvents.Add(evt);
                return result;
            }
        }
        

        public override void Handle(object obj)
        {
            if (obj is CasePlanAddedEvent)
            {
                Handle((CasePlanAddedEvent)obj);
            }
        }

        private void Handle(CasePlanAddedEvent evt)
        {
            Id = evt.Id;
            Version = evt.Version;
            CasePlanId = evt.CasePlanId;
            Name = evt.Name;
            Description = evt.Description;
            CaseOwner = evt.CaseOwner;
            CaseFileId = evt.CaseFileId;
            CreateDateTime = evt.CreateDateTime;
            ExitCriterias = evt.ExitCriterias;
            Elements = evt.Elements;
        }

        private CasePlanElement GetElement(ICollection<CasePlanElement> elements, string id)
        {
            var result = elements.FirstOrDefault(e => e.Id == id);
            if (result != null)
            {
                return result;
            }

            
            foreach(var stage in elements.Where(e => e is PlanItemDefinition).Cast<PlanItemDefinition>().Where(e => e.Type == CaseElementTypes.Stage).Select(e => e.Stage))
            {
                result = GetElement(stage.Elements, id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public override object Clone()
        {
            return new CasePlanAggregate(Id, CasePlanId, Name, Description, CaseOwner, CaseFileId)
            {
                ExitCriterias = ExitCriterias.Select(e => (Criteria)e.Clone()).ToList(),
                CreateDateTime = CreateDateTime,
                Elements = Elements.Select(e => (CasePlanElement)e.Clone()).ToList()
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
            return Id.GetHashCode();
        }

        public static string GetStreamName(string id)
        {
            return $"case-plan-{id}";
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
