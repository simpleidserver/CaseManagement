using System;

namespace CaseManagement.HumanTask.Domains
{
    public class PresentationParameter : ICloneable
    {
        /// <summary>
        /// Uniquely identifies the parameter definition.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Defines its type.
        /// </summary>
        public ParameterTypes Type { get; set; }
        /// <summary>
        /// Define parameter.
        /// Example : htd:getInput("ClaimApprovalRequest")/cust/firstname
        /// </summary>
        public string Expression { get; set; }

        public object Clone()
        {
            return new PresentationParameter
            {
                Name = Name,
                Type = Type,
                Expression = Expression
            };
        }
    }
}