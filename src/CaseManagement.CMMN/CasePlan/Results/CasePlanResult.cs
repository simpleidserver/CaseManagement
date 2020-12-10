using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.CasePlan.Results
{
    public class CasePlanResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseFile { get; set; }
        public string CasePlanId { get; set; }
        public int NbInstances { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int Version { get; set; }

        public static CasePlanResult ToDto(CasePlanAggregate casePlan)
        {
            return new CasePlanResult
            {
                CaseFile = casePlan.CaseFileId,
                CreateDateTime = casePlan.CreateDateTime,
                Description = casePlan.Description,
                NbInstances = casePlan.NbInstances,
                Id = casePlan.AggregateId,
                Name = casePlan.Name,
                Version = casePlan.Version,
                CasePlanId = casePlan.CasePlanId
            };
        }
    }
}
