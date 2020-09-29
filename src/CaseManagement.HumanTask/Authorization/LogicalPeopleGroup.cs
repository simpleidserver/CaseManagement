using System.Collections.Generic;

namespace CaseManagement.HumanTask.Authorization
{
    public class LogicalPeopleGroup
    {
        public LogicalPeopleGroup()
        {
            Metadata = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public ICollection<Member> Members { get; set; }
    }
}
