using System.Collections.Generic;

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
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CMMNWorkflowElementDefinition> Elements { get; set; }

        public static CMMNWorkflowDefinition New(string id, string name, string description, ICollection<CMMNWorkflowElementDefinition> elements)
        {
            return new CMMNWorkflowDefinition(id, name, description, elements);
        }
    }
}
