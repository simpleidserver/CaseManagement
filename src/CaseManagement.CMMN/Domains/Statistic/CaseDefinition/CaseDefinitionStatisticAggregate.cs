using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CaseDefinitionStatisticAggregate : ICloneable
    {
        public CaseDefinitionStatisticAggregate()
        {
            Statistics = new List<CaseElementDefinitionStatistic>();
        }

        public string CaseDefinitionId { get; set; }
        public int NbInstances { get; set; }
        public ICollection<CaseElementDefinitionStatistic> Statistics { get; set; }

        public object Clone()
        {
            return new CaseDefinitionStatisticAggregate
            {
                CaseDefinitionId = CaseDefinitionId,
                NbInstances = NbInstances,
                Statistics = Statistics.Select(s => (CaseElementDefinitionStatistic)s.Clone()).ToList()
            };
        }
    }
}
