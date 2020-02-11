using CaseManagement.CMMN.Infrastructures;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class RoleAggregate : BaseAggregate
    {
        public RoleAggregate()
        {
            UserIds = new List<string>();
        }

        public string Name { get; set; }
        public ICollection<string> UserIds { get; set; }

        public override object Clone()
        {
            return new RoleAggregate
            {
                Id = Id,
                Name = Name,
                UserIds = UserIds.ToList()
            };
        }

        public override void Handle(object obj)
        {
        }
    }
}
