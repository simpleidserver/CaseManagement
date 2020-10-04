using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class UserIdentifiersAssignmentDefinition : PeopleAssignmentDefinition
    {
        public UserIdentifiersAssignmentDefinition()
        {
            UserIdentifiers = new List<string>();
        }

        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.USERIDENTFIERS;
        public ICollection<string> UserIdentifiers { get; set; }

        public override object Clone()
        {
            return new UserIdentifiersAssignmentDefinition
            {
                UserIdentifiers = UserIdentifiers.ToList()
            };
        }
    }
}
