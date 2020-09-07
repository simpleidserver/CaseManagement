using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Builders
{
    public class CasePlanInstanceBuilder : StageInstanceBuilder
    {
        private CasePlanInstanceBuilder(string id, string casePlanName) : base(id, casePlanName)
        {
        }

        public static new CasePlanInstanceBuilder New(string id, string casePlanName)
        {
            return new CasePlanInstanceBuilder(id, casePlanName);
        }

        public new CasePlanInstanceBuilder AddEmptyTask(string id, string name, Action<EmptyTaskInstanceBuilder> callback = null)
        {
            base.AddEmptyTask(id, name, callback);
            return this;
        }

        public new CasePlanInstanceBuilder AddHumanTask(string id, string name, string performerRef, Action<HumanTaskInstanceBuilder> callback = null)
        {
            base.AddHumanTask(id, name, performerRef, callback);
            return this;
        }

        public new CasePlanInstanceBuilder AddStage(string id, string name, Action<StageInstanceBuilder> callback = null)
        {
            base.AddStage(id, name, callback);
            return this;
        }

        public new CasePlanInstanceAggregate Build()
        {
            var result = new CasePlanInstanceAggregate
            {
                Id = Id,
                Name = Name,
                Content = base.InternalBuild() as StageElementInstance
            };
            return result;
        }
    }
}
