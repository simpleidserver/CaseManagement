using System;

namespace CaseManagement.CMMN.Domains
{
    public abstract class CMMNPlanItemDefinition : ICloneable
    {
        public CMMNPlanItemDefinition() { }

        public CMMNPlanItemDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public abstract void Handle(CMMNPlanItemTransitions transition);

        public abstract object Clone();
    }
}
