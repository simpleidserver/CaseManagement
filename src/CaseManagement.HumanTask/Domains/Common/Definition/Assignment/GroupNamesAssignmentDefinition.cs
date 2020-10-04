using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class GroupNamesAssignmentDefinition : PeopleAssignmentDefinition
    {
        public GroupNamesAssignmentDefinition()
        {
            GroupNames = new List<string>();
        }

        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.GROUPNAMES;
        public ICollection<string> GroupNames { get; set; }

        public override object Clone()
        {
            return new GroupNamesAssignmentDefinition
            {
                GroupNames = GroupNames.ToList()
            };
        }
    }
}
