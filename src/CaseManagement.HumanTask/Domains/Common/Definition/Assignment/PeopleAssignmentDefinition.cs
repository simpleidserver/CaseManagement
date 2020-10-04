using System;

namespace CaseManagement.HumanTask.Domains
{
    public abstract class PeopleAssignmentDefinition : ICloneable
    {
        public abstract PeopleAssignmentTypes Type { get; }

        public abstract object Clone();
    }
}
