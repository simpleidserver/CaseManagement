using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Builders
{
    public abstract class CaseEltInstanceBuilder
    {
        public CaseEltInstanceBuilder(string casePlanInstanceId, string eltId, string name)
        {
            CasePlanInstanceId = casePlanInstanceId;
            EltId = eltId;
            Name = name;
        }

        internal string CasePlanInstanceId { get; set; }
        internal string EltId { get; set; }
        internal string Name { get; set; }

        public CaseEltInstance Build()
        {
            var result = InternalBuild();
            result.Id = BuildId();
            result.EltId = EltId;
            result.Name = Name;
            return result;
        }

        protected abstract CaseEltInstance InternalBuild();
        protected abstract string BuildId();
    }

    public abstract class BaseCasePlanItemEltInstanceBuilder : CaseEltInstanceBuilder
    {
        private ICollection<CriteriaBuilder> _entryCriterions;
        private int _version = 0;

        public BaseCasePlanItemEltInstanceBuilder(string casePlanInstanceId, string eltId, string name) : base(casePlanInstanceId, eltId, name)
        {
            _entryCriterions = new List<CriteriaBuilder>();
        }

        protected int Version => _version;

        public BaseCasePlanItemEltInstanceBuilder SetVersion(int version)
        {
            _version = version;
            return this;
        }


        public BaseCasePlanItemEltInstanceBuilder AddEntryCriteria(string name, Action<CriteriaBuilder> callback = null)
        {
            var builder = new CriteriaBuilder(name);
            if (callback != null)
            {
                callback(builder);
            }

            _entryCriterions.Add(builder);
            return this;
        }
        
        protected void SeedCasePlanItem(CaseEltInstance casePlanItem)
        {
            foreach (var entryCritera in _entryCriterions)
            {
                casePlanItem.AddEntryCriteria(entryCritera.Build());
            }
        }
    }

    public abstract class BaseTaskInstanceBuilder : BaseCasePlanItemEltInstanceBuilder
    {
        public BaseTaskInstanceBuilder(string casePlanInstanceId, string eltId, string name) : base(casePlanInstanceId, eltId, name) { }

        protected ManualActivationRule ManualActivationRule { get; set; }
        protected RepetitionRule RepetitionRule { get; set; }

        public BaseTaskInstanceBuilder SetManualActivationRule(string name, CMMNExpression expression)
        {
            ManualActivationRule = new ManualActivationRule
            {
                Expression = expression,
                Name = name
            };
            return this;
        }

        public BaseTaskInstanceBuilder SetRepetitionRule(string name, CMMNExpression expression)
        {
            RepetitionRule = new RepetitionRule
            {
                Condition = expression,
                Name = name
            };
            return this;
        }
    }

    public class EmptyTaskInstanceBuilder : BaseTaskInstanceBuilder
    {
        public EmptyTaskInstanceBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name) { }

        protected override CaseEltInstance InternalBuild()
        {
            var result = new CaseEltInstance
            {
                ManualActivationRule = ManualActivationRule,
                RepetitionRule = RepetitionRule,
                NbOccurrence = Version,
                Type = CasePlanElementInstanceTypes.EMPTYTASK
            };
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return CaseEltInstance.BuildId(CasePlanInstanceId, EltId, Version);
        }
    }

    public class HumanTaskInstanceBuilder : BaseTaskInstanceBuilder
    {
        public HumanTaskInstanceBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name) { }

        public string PerformerRef { get; set; }
        public string Implementation { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }

        protected override CaseEltInstance InternalBuild()
        {
            var result = new CaseEltInstance
            {
                ManualActivationRule = ManualActivationRule,
                RepetitionRule = RepetitionRule,
                NbOccurrence = Version,
                Type = CasePlanElementInstanceTypes.HUMANTASK
            };
            result.UpdatePerformerRef(PerformerRef);
            result.UpdateImplementation(Implementation);
            result.UpdateInputParameters(InputParameters);
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return CaseEltInstance.BuildId(CasePlanInstanceId, EltId, Version);
        }
    }

    public class MilestoneInstanceBuilder : BaseCasePlanItemEltInstanceBuilder
    {
        public MilestoneInstanceBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name)
        {
        }

        protected override CaseEltInstance InternalBuild()
        {
            var result = new CaseEltInstance
            {
                Type = CasePlanElementInstanceTypes.MILESTONE
            };
            result.NbOccurrence = Version;
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return CaseEltInstance.BuildId(CasePlanInstanceId, EltId, Version);
        }
    }

    public class FileItemInstanceBuilder : CaseEltInstanceBuilder
    {
        public FileItemInstanceBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name)
        {
        }

        public string DefinitionType { get; set; }

        protected override CaseEltInstance InternalBuild()
        {
            var result = new CaseEltInstance
            {
                Type = CasePlanElementInstanceTypes.FILEITEM
            };
            result.UpdateDefinitionType(DefinitionType);
            return result;
        }

        protected override string BuildId()
        {
            return CaseEltInstance.BuildId(CasePlanInstanceId, EltId);
        }
    }

    public class TimerEventListenerBuilder : BaseCasePlanItemEltInstanceBuilder
    {
        public TimerEventListenerBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name) { }

        public CMMNExpression TimerExpression { get; set; }

        protected override CaseEltInstance InternalBuild()
        {
            var result = new CaseEltInstance
            {
                NbOccurrence = Version,
                Type = CasePlanElementInstanceTypes.TIMER
            };
            result.UpdateTimerExpression(TimerExpression);
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return CaseEltInstance.BuildId(CasePlanInstanceId, EltId, Version);
        }
    }
}
