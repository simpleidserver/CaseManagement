using System;

namespace CaseManagement.HumanTask.Domains
{
    public class PeopleAssignmentInstance : ICloneable
    {
        public long Id { get; set; }
        public PeopleAssignmentTypes Type { get; set; }
        public PeopleAssignmentUsages Usage { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new PeopleAssignmentInstance
            {
                Id = Id,
                Type = Type,
                Usage = Usage,
                Value = Value
            };
        }
    }
}
