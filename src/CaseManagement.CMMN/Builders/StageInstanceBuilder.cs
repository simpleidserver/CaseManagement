using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class StageInstanceBuilder : BaseTaskInstanceBuilder
    {
        protected StageInstanceBuilder(string casePlanIntanceId, string eltId, string name) : base(casePlanIntanceId, eltId, name)
        {
            Builders = new List<BaseCasePlanItemEltInstanceBuilder>();
        }

        protected ICollection<BaseCasePlanItemEltInstanceBuilder> Builders;

        public StageInstanceBuilder AddEmptyTask(string id, string name, Action<EmptyTaskInstanceBuilder> callback = null)
        {
            var stepBuilder = new EmptyTaskInstanceBuilder(CasePlanInstanceId, id, name);
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        public StageInstanceBuilder AddHumanTask(string id, string name, string performerRef, Action<HumanTaskInstanceBuilder> callback = null)
        {
            var stepBuilder = new HumanTaskInstanceBuilder(CasePlanInstanceId, id, name)
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
            var stepBuilder = new StageInstanceBuilder(CasePlanInstanceId, id, name);
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        public StageInstanceBuilder AddMilestone(string id, string name, Action<MilestoneInstanceBuilder> callback = null)
        {
            var stepBuilder = new MilestoneInstanceBuilder(CasePlanInstanceId, id, name);
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        public StageInstanceBuilder AddTimerEventListener(string id, string name, Action<TimerEventListenerBuilder> callback = null)
        {
            var stepBuilder = new TimerEventListenerBuilder(CasePlanInstanceId, id, name);
            if (callback != null)
            {
                callback(stepBuilder);
            }

            Builders.Add(stepBuilder);
            return this;
        }

        protected override BaseCaseEltInstance InternalBuild()
        {
            var result = new StageElementInstance();
            foreach(var builder in Builders)
            {
                result.AddChild(builder.Build() as BaseCasePlanItemInstance);
            }

            return result;
        }

        protected override string BuildId()
        {
            return StageElementInstance.BuildId(CasePlanInstanceId, EltId, 0);
        }
    }
}