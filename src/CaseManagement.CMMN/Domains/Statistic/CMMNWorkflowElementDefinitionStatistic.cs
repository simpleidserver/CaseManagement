using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowElementDefinitionExecutionHistory : ICloneable
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class CMMNWorkflowElementDefinitionStatistic : ICloneable
    {
        public string ElementDefinitionId { get; set; }
        public int NbInstances { get; set; }

        public object Clone()
        {
            return new CMMNWorkflowElementDefinitionStatistic
            {
                ElementDefinitionId = ElementDefinitionId,
                NbInstances = NbInstances
            };
        }
    }
}
