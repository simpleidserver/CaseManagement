using System;

namespace CaseManagement.HumanTask.Domains
{
    public enum PeopleAssignmentUsages
    {
        POTENTIALOWNER = 0,
        EXCLUDEDOWNER = 1,
        TASKINITIATOR = 2,
        TASKSTAKEHOLDER = 3,
        BUSINESSADMINISTRATOR = 4,
        RECIPIENT = 5
    }

    public class PeopleAssignmentDefinition : ICloneable
    {
        public string Id { get; set; }
        public PeopleAssignmentTypes Type { get; set; }
        public PeopleAssignmentUsages Usage { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new PeopleAssignmentDefinition
            {
                Id = Id,
                Type = Type,
                Usage = Usage,
                Value = Value
            };
        }
    }
}
