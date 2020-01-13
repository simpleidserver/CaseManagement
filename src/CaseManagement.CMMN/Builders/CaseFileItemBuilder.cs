using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CaseFileItemBuilder
    {
        private readonly CaseFileItemDefinition _caseFileItemDefinition;

        public CaseFileItemBuilder(CaseFileItemDefinition caseFileItemDefinition)
        {
            _caseFileItemDefinition = caseFileItemDefinition;
        }
        
        public CaseFileItemBuilder SetMultiplicity(Multiplicities multiplicity)
        {
            _caseFileItemDefinition.Multiplicity = multiplicity;
            return this;
        }

        public CaseFileItemBuilder SetDefinition(string definition)
        {
            _caseFileItemDefinition.Definition = definition;
            return this;
        }
    }
}
