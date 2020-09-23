using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class ExecutionPath : ICloneable
    {
        public ExecutionPath()
        {
            Pointers = new List<ExecutionPointer>();
        }


        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<ExecutionPointer> Pointers { get; set; }
        public ICollection<ExecutionPointer> ActivePointers => Pointers.Where(_ => _.IsActive).ToList();

        public object Clone()
        {
            return new ExecutionPath
            {
                Id = Id,
                CreateDateTime = CreateDateTime,
                Pointers = Pointers.Select(_ => (ExecutionPointer)_.Clone()).ToList()
            };
        }
    }
}
