using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public class CasePlanInstanceBuilder : StageInstanceBuilder
    {
        private ICollection<FileItemInstanceBuilder> _builders;

        private CasePlanInstanceBuilder(string id, string casePlanName) : base(id, id, casePlanName) 
        {
            _builders = new List<FileItemInstanceBuilder>();
        }

        public static CasePlanInstanceBuilder New(string id, string casePlanName)
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

        public new CasePlanInstanceBuilder AddMilestone(string id, string name, Action<MilestoneInstanceBuilder> callback = null)
        {
            base.AddMilestone(id, name, callback);
            return this;
        }

        public CasePlanInstanceBuilder AddFileItem(string id, string name, Action<FileItemInstanceBuilder> callback = null)
        {
            var fileItemBuilder = new FileItemInstanceBuilder(CasePlanInstanceId, id, name);
            if (callback != null)
            {
                callback(fileItemBuilder);
            }

            _builders.Add(fileItemBuilder);
            return this;
        }

        public new CasePlanInstanceBuilder AddStage(string id, string name, Action<StageInstanceBuilder> callback = null)
        {
            base.AddStage(id, name, callback);
            return this;
        }

        public new CasePlanInstanceBuilder AddTimerEventListener(string id, string name, Action<TimerEventListenerBuilder> callback = null)
        {
            base.AddTimerEventListener(id, name, callback);
            return this;
        }

        public new CasePlanInstanceAggregate Build()
        {
            var files = new List<CaseFileItemInstance>();
            foreach(var builder in _builders)
            {
                files.Add(builder.Build() as CaseFileItemInstance);
            }

            var result = CasePlanInstanceAggregate.New(CasePlanInstanceId, base.InternalBuild() as StageElementInstance, files);
            return result;
        }
    }
}
