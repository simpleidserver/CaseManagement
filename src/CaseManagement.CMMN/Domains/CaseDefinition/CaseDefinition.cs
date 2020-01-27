using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CaseDefinition
    {
        public CaseDefinition(string id, string name, string description, ICollection<CaseElementDefinition> elements)
        {
            Id = id;
            Name = name;
            Description = description;
            Elements = elements;
            ExitCriterias = new List<Criteria>();
            CaseInstanceIds = new List<string>();
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseOwner { get; set; }
        public string CaseFileId { get; set; }
        public ICollection<string> CaseInstanceIds { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<Criteria> ExitCriterias { get; set; }
        public ICollection<CaseElementDefinition> Elements { get; set; }

        public CaseElementDefinition GetElement(string id)
        {
            return GetElement(Elements, id);
        }

        public static CaseDefinition New(string id, string name, string description, ICollection<CaseElementDefinition> elements, string caseOwner = null)
        {
            var result = new CaseDefinition(id, name, description, elements)
            {
                CreateDateTime = DateTime.UtcNow,
                CaseOwner = caseOwner
            };
            return result;
        }


        private CaseElementDefinition GetElement(ICollection<CaseElementDefinition> elements, string id)
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
    }
}
