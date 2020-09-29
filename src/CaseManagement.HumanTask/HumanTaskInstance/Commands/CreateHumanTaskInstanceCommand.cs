using MediatR;
using System;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    public class CreateHumanTaskInstanceCommand : IRequest<string>
    {
        public string HumanTaskName { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
        public int? Priority { get; set; }
        public DateTime? ActivationDeferralTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public CreateHumanTaskInstanceAssignPeople PeopleAssignment { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }

        public class CreateHumanTaskInstanceAssignPeople
        {
            public AssignPeople PotentialOwner { get; set; }
            public AssignPeople ExcludedOwner { get; set; }
            public AssignPeople TaskStakeHolder { get; set; }
            public AssignPeople BusinessAdministrator { get; set; }
            public AssignPeople Recipient { get; set; }
        }

        public class AssignPeople
        {
            public AssignLogicalPeopleGroup LogicalPeopleGroup { get; set; }
            public AssignUserIdentifiers UserIdentifiers { get; set; }
            public AssignGroupNames GroupNames { get; set; }
            public AssignExpression Expression { get; set; }
        }

        public class AssignUserIdentifiers
        {
            public ICollection<string> UserIdentifiers { get; set; }
        }

        public class AssignGroupNames
        {
            public ICollection<string> GroupNames { get; set; }
        }

        public class AssignLogicalPeopleGroup
        {
            public string LogicalPeopleGroup { get; set; }
            public Dictionary<string, string> Arguments { get; set; }
        }

        public class AssignExpression
        {
            public string Expression { get; set; }
        }
    }
}
