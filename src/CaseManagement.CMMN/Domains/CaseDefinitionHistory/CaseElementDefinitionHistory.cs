using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseElementDefinitionHistory : ICloneable
    {
        public string CaseElementDefinitionId { get; set; }
        public int NbInstances { get; set; }

        public object Clone()
        {
            return new CaseElementDefinitionHistory
            {
                CaseElementDefinitionId = CaseElementDefinitionId,
                NbInstances = NbInstances
            };
        }
    }
}
