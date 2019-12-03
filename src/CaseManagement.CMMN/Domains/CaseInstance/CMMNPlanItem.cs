using CaseManagement.Workflow.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItem : ProcessFlowInstanceElement
    {
        private CMMNPlanItem(string id, string name, CMMNPlanItemDefinition planItemDefinition) : base(id, name)
        {
            EntryCriterions = new List<CMMNCriterion>();
            ExitCriterions = new List<CMMNCriterion>();
            TransitionHistories = new List<CMMNPlanItemStateHistory>();
            PlanItemDefinition = planItemDefinition;
        }

        /// <summary>
        /// The PlanItemControl controls aspects of the behavior of instances of the PlanItem object. [0...1]
        /// </summary>
        public CMMNPlanItemControl PlanItemControl { get; set; }
        /// <summary>
        /// Reference to the corresponding PlanItemDefinition object. [1...1]
        /// </summary>
        public CMMNPlanItemDefinition PlanItemDefinition { get; set; }
        /// <summary>
        /// Zero or more EntryCriterion for that PlanItem. [0...*].
        /// An EntryCriterion represents the condition for a PlanItem to become available.
        /// </summary>
        public ICollection<CMMNCriterion> EntryCriterions { get; set; }
        /// <summary>
        /// An ExitCriterion represents the condition for a PlanItem to terminate. [0...*]
        /// </summary>
        public ICollection<CMMNCriterion> ExitCriterions { get; set; }
        public ICollection<CMMNPlanItemStateHistory> TransitionHistories { get; set; }
        
        public void Create()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.Create);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.Create));
        }

        public void Enable()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.Enable);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.Enable));
        }

        public void ManualStart()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.ManualStart);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.ManualStart));
        }

        public void Occur()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.Occur);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.Occur));
        }

        public void Start()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.Start);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.Start));
        }

        public void Disable()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.Disable);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.Disable));
        }

        public void Complete()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.Complete);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.Complete));
        }

        public void Terminate()
        {
            PlanItemDefinition.Handle(CMMNPlanItemTransitions.Terminate);
            TransitionHistories.Add(new CMMNPlanItemStateHistory(DateTime.UtcNow, CMMNPlanItemTransitions.Terminate));
        }

        public static CMMNPlanItem New(string id, string name, CMMNPlanItemDefinition planItemDef)
        {
            var result = new CMMNPlanItem(id, name, planItemDef);
            return result;
        }

        public override void HandleLaunch()
        {
            Create();
        }

        public override object Clone()
        {
            return new CMMNPlanItem(Id, Name, (CMMNPlanItemDefinition)PlanItemDefinition.Clone())
            {
                Status = Status,
                PlanItemControl = PlanItemControl == null ? null : (CMMNPlanItemControl)PlanItemControl.Clone(),
                EntryCriterions = EntryCriterions.Select(e => (CMMNCriterion)e.Clone()).ToList(),
                ExitCriterions = EntryCriterions.Select(e => (CMMNCriterion)e.Clone()).ToList(),
                TransitionHistories = TransitionHistories.Select(e => (CMMNPlanItemStateHistory)e.Clone()).ToList()
            };
        }
    }
}