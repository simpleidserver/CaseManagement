using CaseManagement.Workflow.Infrastructure;
using System;

namespace CaseManagement.CMMN.Domains
{
    public abstract class CMMNPlanItemDefinition : ICloneable
    {
        public CMMNPlanItemDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public abstract void Handle(DomainEvent cmmnPlanItemEvent);

        public abstract object Clone();
    }
}
