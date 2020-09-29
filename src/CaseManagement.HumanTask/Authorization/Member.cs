using System.Collections.Generic;

namespace CaseManagement.HumanTask.Authorization
{
    public class Member
    {
        public ICollection<KeyValuePair<string, string>> Claims { get; set; }
    }
}
