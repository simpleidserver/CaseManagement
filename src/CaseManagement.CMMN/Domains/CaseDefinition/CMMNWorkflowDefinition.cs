using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowDefinition
    {
        public CMMNWorkflowDefinition(string id, string name, string description, ICollection<CMMNPlanItemDefinition> planItems)
        {
            Id = id;
            Name = name;
            Description = description;
            PlanItems = planItems;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CMMNPlanItemDefinition> PlanItems { get; set; }

        public static CMMNWorkflowDefinition New(string id, string name, string description, ICollection<CMMNPlanItemDefinition> planItems)
        {
            return new CMMNWorkflowDefinition(id, name, description, planItems);
        }
    }
}
