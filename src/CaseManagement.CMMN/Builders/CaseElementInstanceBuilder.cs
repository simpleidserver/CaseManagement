using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Builders
{
    public class CaseElementInstanceBuilder
    {
        private ICollection<CriteriaBuilder> _entryCriterions;

        public CaseElementInstanceBuilder(string id, string name)
        {
            Id = id;
            Name = name;
            _entryCriterions = new List<CriteriaBuilder>();
        }

        internal string Id { get; set; }
        internal string Name { get; set; }

        public CaseElementInstanceBuilder AddEntryCriteria(string name, Action<CriteriaBuilder> callback = null)
        {
            var builder = new CriteriaBuilder(name);
            if (callback != null)
            {
                callback(builder);
            }

            _entryCriterions.Add(builder);
            return this;
        }

        public CasePlanElementInstance Build()
        {
            var result = InternalBuild();
            foreach(var entryCritera in _entryCriterions)
            {
                result.EntryCriterions.Add(entryCritera.Build());
            }

            return result;
        }

        protected virtual CasePlanElementInstance InternalBuild()
        {
            return null;
        }
    }

    public class BaseTaskInstanceBuilder : CaseElementInstanceBuilder
    {
        public BaseTaskInstanceBuilder(string id, string name) : base(id, name) { }

        protected ManualActivationRule ManualActivationRule { get; set; }

        public BaseTaskInstanceBuilder SetManualActivationRule(string name, CMMNExpression expression)
        {
            ManualActivationRule = new ManualActivationRule
            {
                Expression = expression,
                Name = name
            };
            return this;
        }
    }

    public class EmptyTaskInstanceBuilder : BaseTaskInstanceBuilder
    {
        public EmptyTaskInstanceBuilder(string id, string name) : base(id, name) { }

        protected override CasePlanElementInstance InternalBuild()
        {
            return new EmptyTaskElementInstance
            {
                Id = Id,
                Name = Name,
                ManualActivationRule = ManualActivationRule
            };
        }
    }

    public class HumanTaskInstanceBuilder : BaseTaskInstanceBuilder
    {
        public HumanTaskInstanceBuilder(string id, string name) : base(id, name) { }

        public string PerformerRef { get; set; }

        protected override CasePlanElementInstance InternalBuild()
        {
            return new HumanTaskElementInstance
            {
                Id = Id,
                Name = Name,
                PerformerRef = PerformerRef,
                ManualActivationRule = ManualActivationRule
            };
        }
    }
}
