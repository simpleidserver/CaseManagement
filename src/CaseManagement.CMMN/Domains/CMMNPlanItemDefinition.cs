using CaseManagement.CMMN.Domains.Events;

namespace CaseManagement.CMMN.Domains
{
    public abstract class CMMNPlanItemDefinition
    {
        public CMMNPlanItemDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public abstract void Handle(DomainEvent cmmnPlanItemEvent);
    }
}
