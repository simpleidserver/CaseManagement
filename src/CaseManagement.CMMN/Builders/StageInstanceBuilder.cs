using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Builders
{
    public class StageInstanceBuilder
    {

        protected StageInstanceBuilder(string id, string name)
        {
            Id = id;
            Name = name;
            Builders = new List<CaseElementInstanceBuilder>();
        }

        protected string Id { get; set; }
        protected string Name { get; set; }

        protected ICollection<CaseElementInstanceBuilder> Builders;

        public StageInstanceBuilder AddEmptyTask(string id, string name, Action<EmptyTaskInstanceBuilder> callback = null)
        {
            var stepBuilder = new EmptyTaskInstanceBuilder
            {
                Name = name,
                Id = id
            };
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        public StageInstanceBuilder AddHumanTask(string id, string name, string performerRef, Action<HumanTaskInstanceBuilder> callback = null)
        {
            var stepBuilder = new HumanTaskInstanceBuilder
            {
                Name = name,
                Id = id,
                PerformerRef = performerRef
            };
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

        public StageElementInstance Build()
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
