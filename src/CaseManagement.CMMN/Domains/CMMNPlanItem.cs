using CaseManagement.CMMN.Domains.Events;
using CaseManagement.Workflow.Domains;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItem : ProcessFlowInstanceElement
    {
        private CMMNPlanItem(string id, string name, CMMNPlanItemDefinition planItemDefinition) : base(id, name)
        {
            EntryCriterions = new List<CMMNCriterion>();
            ExitCriterions = new List<CMMNCriterion>();
            Events = new List<CMMNPlanItemTransitionEvent>();
            var evt = new CMMNPlanItemCreated();
            Events.Add(evt);
            PlanItemDefinition = planItemDefinition;
            planItemDefinition.Handle(evt);
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
        public ICollection<CMMNPlanItemTransitionEvent> Events { get; set; }
        
        public void Enable()
        {
            var evt = new CMMNPlanItemEnabled();
            Events.Add(evt);
            PlanItemDefinition.Handle(evt);
        }

        public void ManualStart()
        {
            var evt = new CMMNPlanItemManuallyStarted();
            Events.Add(evt);
            PlanItemDefinition.Handle(evt);
        }

        public void Start()
        {
            var evt = new CMMNPlanItemStarted();
            Events.Add(evt);
            PlanItemDefinition.Handle(evt);
        }

        public void Disable()
        {

        }

        public void Complete()
        {
            var evt = new CMMNPlanItemCompleted();
            Events.Add(evt);
            PlanItemDefinition.Handle(evt);
        }

        public void Terminate()
        {
            var evt = new CMMNPlanItemTerminated();
            Events.Add(evt);
            PlanItemDefinition.Handle(evt);
        }

        public static CMMNPlanItem New(string id, string name, CMMNPlanItemDefinition planItemDef)
        {
            var result = new CMMNPlanItem(id, name, planItemDef);
            return result;
        }
    }
}