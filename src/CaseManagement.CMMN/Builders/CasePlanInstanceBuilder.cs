using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Builders
{
    public class CasePlanInstanceBuilder : StageInstanceBuilder
    {
        private CasePlanInstanceBuilder(string id, string casePlanName) : base(id, casePlanName)
        {
        }

        public static CasePlanInstanceBuilder New(string id, string casePlanName)
        {
            return new CasePlanInstanceBuilder(id, casePlanName);
        }

        public new CasePlanInstanceBuilder AddEmptyTask(string id, string name, Action<CaseElementInstanceBuilder> callback = null)
        {
            base.AddEmptyTask(id, name, callback);
            return this;
        }

        public new CasePlanInstanceBuilder AddHumanTask(string id, string name, string performerRef, Action<CaseElementInstanceBuilder> callback = null)
        {
            base.AddHumanTask(id, name, performerRef, callback);
            return this;
        }

        public new CasePlanInstanceAggregate Build()
        {
            var result = new CasePlanInstanceAggregate
            {
                Id = Id,
                Name = Name,
                Content = base.Build()
            };
            return result;
        }
    }
}
