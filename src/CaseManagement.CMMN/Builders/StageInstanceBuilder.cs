using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Builders
{
    public class StageInstanceBuilder : CaseElementInstanceBuilder
    {

        protected StageInstanceBuilder(string id, string name) : base(id, name)
        {
            Builders = new List<CaseElementInstanceBuilder>();
        }

        protected ICollection<CaseElementInstanceBuilder> Builders;

        public StageInstanceBuilder AddEmptyTask(string id, string name, Action<EmptyTaskInstanceBuilder> callback = null)
        {
            var stepBuilder = new EmptyTaskInstanceBuilder(id, name);
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        public StageInstanceBuilder AddHumanTask(string id, string name, string performerRef, Action<HumanTaskInstanceBuilder> callback = null)
        {
            var stepBuilder = new HumanTaskInstanceBuilder(id, name)
            {
                PerformerRef = performerRef
            };
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        public StageInstanceBuilder AddStage(string id, string name, Action<StageInstanceBuilder> callback = null)
        {
            var stepBuilder = new StageInstanceBuilder(id, name);
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        public static StageInstanceBuilder New(string id, string name)
        {
            return new StageInstanceBuilder(id, name);
        }

        protected override CasePlanElementInstance InternalBuild()
        {
            var result = new StageElementInstance
            {
                Id = Id,
                Name = Name
            };
            foreach(var builder in Builders)
            {
                result.AddChild(builder.Build());
            }

            return result;
        }
    }
}