using System.Collections.Generic;

namespace CaseManagement.HumanTask.Persistence.Parameters
{
    public class UserClaims
    {
        public string UserIdentifier { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<string> LogicalGroups { get; set; }
    }
}
