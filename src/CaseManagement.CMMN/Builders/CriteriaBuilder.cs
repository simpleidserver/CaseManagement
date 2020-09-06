using CaseManagement.CMMN.Domains;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Builders
{
    public class CriteriaBuilder
    {
        private readonly string _name;
        private ICollection<PlanItemOnPart> _planItems;

        public CriteriaBuilder(string name)
        {
            _name = name;
            _planItems = new List<PlanItemOnPart>();
        }

        public CriteriaBuilder AddPlanItemOnPart(string sourceRef, CMMNTransitions transition)
        {
            _planItems.Add(new PlanItemOnPart
            {
                SourceRef = sourceRef,
                StandardEvent = transition 
            });
            return this;
        }

        public Criteria Build()
        {
            var result = new Criteria(_name);
            result.SEntry = new SEntry(_name)
            {
                PlanItemOnParts = _planItems
            };
            return result;
        }
    }
}
