using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class BPMNInterface : BaseElement, ICloneable
    {
        public BPMNInterface()
        {
            Operations = new List<Operation>();
        }

        /// <summary>
        /// The descriptive name of the element.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This attribute allows to reference a concrete artifact in the underlying implementation technology representing that interface, such as a WSDL porttype.
        /// </summary>
        public string ImplementationRef { get; set; }
        /// <summary>
        /// This attribute specifies operations that are defined as part of the Interface.
        /// An Interface has at least one Operation.
        /// </summary>
        public virtual ICollection<Operation> Operations { get; set; }

        public object Clone()
        {
            return new BPMNInterface
            {
                EltId = EltId,
                Name = Name,
                Operations = Operations.Select(_ => (Operation)_.Clone()).ToList(),
                ImplementationRef = ImplementationRef
            };
        }
    }
}
