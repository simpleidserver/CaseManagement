using CaseManagement.CMMN.Domains;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class CriteriaBuilder
    {
        private readonly string _name;
        private ICollection<PlanItemOnPart> _planItems;
        private ICollection<CaseFileItemOnPart> _fileItems;
        private IfPart _ifPart;

        public CriteriaBuilder(string name)
        {
            _name = name;
            _planItems = new List<PlanItemOnPart>();
            _fileItems = new List<CaseFileItemOnPart>();
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

        public CriteriaBuilder AddFileItemOnPart(string sourceRef, CMMNTransitions transition)
        {
            _fileItems.Add(new CaseFileItemOnPart
            {
                SourceRef = sourceRef,
                StandardEvent = transition
            });
            return this;
        }

        public CriteriaBuilder SetIfPart(string condition)
        {
            _ifPart = new IfPart
            {
                Condition = condition
            };
            return this;
        }

        public Criteria Build()
        {
            var result = new Criteria(_name);
            result.SEntry = new SEntry(_name)
            {
                PlanItemOnParts = _planItems,
                FileItemOnParts = _fileItems,
                IfPart = _ifPart
            };
            return result;
        }
    }
}
