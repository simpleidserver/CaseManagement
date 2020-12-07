using System;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class Operation : BaseElement, ICloneable
    {
        /// <summary>
        /// The descriptive name of the element.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This attribute allows to reference a concrete artifact in the underlying implementation technology representing that operation, such as WSDL operation.
        /// </summary>
        public string ImplementationRef { get; set; }
        /// <summary>
        /// Specifies the input Message of the Operation.
        /// An operation has exactly one input Message.
        /// </summary>
        public string InMessageRef { get; set; }
        /// <summary>
        /// An has at most one input Message.
        /// </summary>
        public string OutMessageRef { get; set; }

        public object Clone()
        {
            return new Operation
            {
                ImplementationRef = ImplementationRef,
                Name = Name,
                InMessageRef = InMessageRef,
                OutMessageRef = OutMessageRef
            };
        }
    }
}
