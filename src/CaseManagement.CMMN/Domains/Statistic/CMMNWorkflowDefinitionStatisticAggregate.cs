using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowDefinitionStatisticAggregate : ICloneable
    {
        public CMMNWorkflowDefinitionStatisticAggregate()
        {
            Statistics = new List<CMMNWorkflowElementDefinitionStatistic>();
        }

        public string WorkflowDefinitionId { get; set; }
        public int NbInstances { get; set; }
        public ICollection<CMMNWorkflowElementDefinitionStatistic> Statistics { get; set; }

        public object Clone()
        {
            return new CMMNWorkflowDefinitionStatisticAggregate
            {
                WorkflowDefinitionId = WorkflowDefinitionId,
                NbInstances = NbInstances,
                Statistics = Statistics.Select(s => (CMMNWorkflowElementDefinitionStatistic)s.Clone()).ToList()
            };
        }
    }
}
