using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CaseDefinitionHistoryAggregate : ICloneable
    {
        public CaseDefinitionHistoryAggregate()
        {
            Statistics = new List<CaseElementDefinitionHistory>();
        }

        public string CaseDefinitionId { get; set; }
        public int NbInstances { get; set; }
        public ICollection<CaseElementDefinitionHistory> Statistics { get; set; }

        public object Clone()
        {
            return new CaseDefinitionHistoryAggregate
            {
                CaseDefinitionId = CaseDefinitionId,
                NbInstances = NbInstances,
                Statistics = Statistics.Select(s => (CaseElementDefinitionHistory)s.Clone()).ToList()
            };
        }
    }
}
