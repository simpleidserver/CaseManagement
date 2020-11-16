using System;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class ExecutionPathModel
    {
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public virtual ICollection<ExecutionPointerModel> Pointers { get; set; }
    }
}
