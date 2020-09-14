using CaseManagement.CMMN.Domains;
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

        public BaseCaseEltInstance Build()
        {
            var result = InternalBuild();
            result.Id = BuildId();
            result.EltId = EltId;
            result.Name = Name;
            return result;
        }

        protected abstract BaseCaseEltInstance InternalBuild();
        protected abstract string BuildId();
    }

    public abstract class BaseCasePlanItemEltInstanceBuilder : CaseEltInstanceBuilder
    {
        private ICollection<CriteriaBuilder> _entryCriterions;

        public BaseCasePlanItemEltInstanceBuilder(string casePlanInstanceId, string eltId, string name) : base(casePlanInstanceId, eltId, name)
        {
            _entryCriterions = new List<CriteriaBuilder>();
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
        
        protected void SeedCasePlanItem(BaseCasePlanItemInstance casePlanItem)
        {
            foreach (var entryCritera in _entryCriterions)
            {
                casePlanItem.EntryCriterions.Add(entryCritera.Build());
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

        protected override BaseCaseEltInstance InternalBuild()
        {
            var result = new EmptyTaskElementInstance
            {
                ManualActivationRule = ManualActivationRule,
                RepetitionRule = RepetitionRule
            };
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return EmptyTaskElementInstance.BuildId(CasePlanInstanceId, EltId, 0);
        }
    }

    public class HumanTaskInstanceBuilder : BaseTaskInstanceBuilder
    {
        public HumanTaskInstanceBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name) { }

        public string PerformerRef { get; set; }

        protected override BaseCaseEltInstance InternalBuild()
        {
            var result = new HumanTaskElementInstance
            {
                PerformerRef = PerformerRef,
                ManualActivationRule = ManualActivationRule,
                RepetitionRule = RepetitionRule
            };
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return HumanTaskElementInstance.BuildId(CasePlanInstanceId, EltId, 0);
        }
    }

    public class MilestoneInstanceBuilder : BaseCasePlanItemEltInstanceBuilder
    {
        public MilestoneInstanceBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name)
        {
        }

        protected override BaseCaseEltInstance InternalBuild()
        {
            var result = new MilestoneElementInstance();
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return MilestoneElementInstance.BuildId(CasePlanInstanceId, EltId, 0);
        }
    }

    public class FileItemInstanceBuilder : CaseEltInstanceBuilder
    {
        public FileItemInstanceBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name)
        {
        }

        public string DefinitionType { get; set; }

        protected override BaseCaseEltInstance InternalBuild()
        {
            return new CaseFileItemInstance
            {
                DefinitionType = DefinitionType
            };
        }

        protected override string BuildId()
        {
            return CaseFileItemInstance.BuildId(CasePlanInstanceId, EltId);
        }
    }

    public class TimerEventListenerBuilder : BaseCasePlanItemEltInstanceBuilder
    {
        public TimerEventListenerBuilder(string casePlanInstanceId, string id, string name) : base(casePlanInstanceId, id, name) { }

        public CMMNExpression TimerExpression { get; set; }

        protected override BaseCaseEltInstance InternalBuild()
        {
            var result = new TimerEventListener
            {
                TimerExpression = TimerExpression
            };
            SeedCasePlanItem(result);
            return result;
        }

        protected override string BuildId()
        {
            return TimerEventListener.BuildId(CasePlanInstanceId, EltId, 0);
        }
    }
}
