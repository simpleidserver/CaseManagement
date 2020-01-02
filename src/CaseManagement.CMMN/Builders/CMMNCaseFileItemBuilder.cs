using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNCaseFileItemBuilder
    {
        private readonly CMMNCaseFileItemDefinition _caseFileItemDefinition;

        public CMMNCaseFileItemBuilder(CMMNCaseFileItemDefinition caseFileItemDefinition)
        {
            _caseFileItemDefinition = caseFileItemDefinition;
        }
        
        public CMMNCaseFileItemBuilder SetMultiplicity(CMMNMultiplicities multiplicity)
        {
            _caseFileItemDefinition.Multiplicity = multiplicity;
            return this;
        }

        public CMMNCaseFileItemBuilder SetDefinition(string definition)
        {
            _caseFileItemDefinition.Definition = definition;
            return this;
        }
    }
}
