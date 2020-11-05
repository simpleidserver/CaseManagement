using CaseManagement.HumanTask.Domains;

namespace CaseManagement.HumanTask.Common
{
    public class AssignPeople
    {
        public PeopleAssignmentTypes Type { get; set; }
        public PeopleAssignmentUsages Usage { get; set; }
        public string Value { get; set; }
    }
}
