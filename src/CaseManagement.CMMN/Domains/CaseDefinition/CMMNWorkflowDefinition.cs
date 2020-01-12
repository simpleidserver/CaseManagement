using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowDefinition
    {
        public CMMNWorkflowDefinition(string id, string name, string description, ICollection<CMMNWorkflowElementDefinition> elements)
        {
            Id = id;
            Name = name;
            Description = description;
            Elements = elements;
            ExitCriterias = new List<CMMNCriterion>();
            CaseInstanceIds = new List<string>();
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseFileId { get; set; }
        public ICollection<string> CaseInstanceIds { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<CMMNCriterion> ExitCriterias { get; set; }
        public ICollection<CMMNWorkflowElementDefinition> Elements { get; set; }

        public CMMNWorkflowElementDefinition GetElement(string id)
        {
            return GetElement(Elements, id);
        }

        public static CMMNWorkflowDefinition New(string id, string name, string description, ICollection<CMMNWorkflowElementDefinition> elements)
        {
            var result = new CMMNWorkflowDefinition(id, name, description, elements)
            {
                CreateDateTime = DateTime.UtcNow
            };
            return result;
        }

        private CMMNWorkflowElementDefinition GetElement(ICollection<CMMNWorkflowElementDefinition> elements, string id)
        {
            var result = elements.FirstOrDefault(e => e.Id == id);
            if (result != null)
            {
                return result;
            }

            
            foreach(var stage in elements.Where(e => e is CMMNPlanItemDefinition).Cast<CMMNPlanItemDefinition>().Where(e => e.Type == CMMNWorkflowElementTypes.Stage).Select(e => e.Stage))
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
