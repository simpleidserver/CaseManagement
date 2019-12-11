using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNCaseFileItemDefinition : ICloneable
    {
        public CMMNCaseFileItemDefinition(string definitionType)
        {
            DefinitionType = definitionType;
        }

        public string DefinitionType { get; set; }

        public object Clone()
        {
            return new CMMNCaseFileItemDefinition(DefinitionType);
        }
    }
}
