using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class LogicalPeopleGroupAssignmentDefinition : PeopleAssignmentDefinition
    {
        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.LOGICALPEOPLEGROUP;
        public string LogicalPeopleGroup { get; set; }
        public Dictionary<string, string> Arguments { get; set; }

        public override object Clone()
        {
            return new LogicalPeopleGroupAssignmentDefinition
            {
                LogicalPeopleGroup = LogicalPeopleGroup,
                Arguments = Arguments.ToDictionary(c => c.Key, c => c.Value)
            };
        }
    }
}
