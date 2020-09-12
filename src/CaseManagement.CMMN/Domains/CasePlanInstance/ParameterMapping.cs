using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class ParameterMapping : ICloneable
    {
        /// <summary>
        /// The transformation Expression transforms the parameter referred to by sourceRef to the parameter referred to by targetRef. [0...1].
        /// </summary>
        public CMMNExpression Transformation { get; set; }
        /// <summary>
        /// One source Parameter. [1...1]
        /// </summary>
        public string SourceRef { get; set; }
        /// <summary>
        /// One target Parameter. [1...1]
        /// </summary>
        public string TargetRef { get; set; }

        public object Clone()
        {
            return new ParameterMapping
            {
                SourceRef = SourceRef,
                TargetRef = TargetRef,
                Transformation = Transformation == null ? null : (CMMNExpression)Transformation.Clone()
            };
        }
    }
}
