using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CaseWorkerTaskRole : ICloneable
    {
        public string RoleId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }

        public object Clone()
        {
            return new CaseWorkerTaskRole
            {
                RoleId = RoleId,
                Claims = Claims.ToList()
            };
        }
    }
}
