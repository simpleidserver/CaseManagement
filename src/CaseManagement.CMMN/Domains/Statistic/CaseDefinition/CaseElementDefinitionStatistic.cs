using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseElementDefinitionStatistic : ICloneable
    {
        public string CaseElementDefinitionId { get; set; }
        public int NbInstances { get; set; }

        public object Clone()
        {
            return new CaseElementDefinitionStatistic
            {
                CaseElementDefinitionId = CaseElementDefinitionId,
                NbInstances = NbInstances
            };
        }
    }
}
