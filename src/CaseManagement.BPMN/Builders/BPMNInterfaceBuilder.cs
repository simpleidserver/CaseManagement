using CaseManagement.BPMN.Domains;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Builders
{
    public class BPMNInterfaceBuilder
    {
        private readonly string _id;
        private readonly string _name;
        private readonly string _implementationRef;
        private readonly ICollection<Operation> _operations;
        
        public BPMNInterfaceBuilder(string id, string name, string implementationRef)
        {
            _id = id;
            _name = name;
            _implementationRef = implementationRef;
            _operations = new List<Operation>();
        }

        public BPMNInterfaceBuilder AddOperation(string id, string name, string inMessageRef, string implementationRef, string outMessageRef = null)
        {
            _operations.Add(new Operation { Id = id, Name = name, InMessageRef = inMessageRef, ImplementationRef = implementationRef, OutMessageRef = outMessageRef });
            return this;
        }

        public BPMNInterface Build()
        {
            return new BPMNInterface
            {
                Id = _id,
                Name = _name,
                ImplementationRef = _implementationRef,
                Operations = _operations
            };
        }
    }
}
