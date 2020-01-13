using System;

namespace CaseManagement.CMMN.Domains
{
    public class ParameterMapping : ICloneable
    {
        /// <summary>
        /// The transformation Expression transforms the parameter referred to by sourceRef to the parameter referred to by targetRef. [0...1].
        /// </summary>
        public CMMNExpression Transformation { get; set; }
        /// <summary>
        /// One source Parameter. [1...1]
        /// </summary>
        public CMMNParameter SourceRef { get; set; }
        /// <summary>
        /// One target Parameter. [1...1]
        /// </summary>
        public CMMNParameter TargetRef { get; set; }

        public object Clone()
        {
            return new ParameterMapping
            {
                SourceRef = SourceRef == null ? null : (CMMNParameter)SourceRef.Clone(),
                TargetRef = TargetRef == null ? null : (CMMNParameter)TargetRef.Clone(),
                Transformation = Transformation == null ? null : (CMMNExpression)Transformation.Clone()
            };
        }
    }
}
