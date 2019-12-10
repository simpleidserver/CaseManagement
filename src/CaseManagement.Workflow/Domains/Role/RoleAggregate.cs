using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class RoleAggregate : ICloneable
    {
        public RoleAggregate()
        {
            UserIds = new List<string>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> UserIds { get; set; }

        public object Clone()
        {
            return new RoleAggregate
            {
                Id = Id,
                Name = Name,
                UserIds = UserIds.ToList()
            };
        }
    }
}
