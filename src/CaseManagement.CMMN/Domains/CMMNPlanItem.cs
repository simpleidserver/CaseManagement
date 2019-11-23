using CaseManagement.CMMN.Domains.Events;
using CaseManagement.Workflow.Domains;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItem : ProcessFlowInstanceElement
    {
        private CMMNPlanItem(string id, string name, CMMNPlanItemDefinition planItemDefinition) : base(id, name)
        {
            SEntries = new List<CMMNSEntry>();
            Events = new List<CMMNPlanItemTransitionEvent>();
            var evt = new CMMNPlanItemCreated();
            Events.Add(evt);
            PlanItemDefinition = planItemDefinition;
            planItemDefinition.Handle(evt);
        }

        /// <summary>
        /// [0...1]
        /// </summary>
        public CMMNPlanItemControl PlanItemControl { get; set; }
        /// <summary>
        /// [1...1]
        /// </summary>
        public CMMNPlanItemDefinition PlanItemDefinition { get; set; }
        /// <summary>
        /// [0...*] // EntryCriterion TODO !!
        /// </summary>
        public ICollection<CMMNSEntry> SEntries { get; set; }
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

        public static CMMNPlanItem New(string id, string name, CMMNPlanItemDefinition planItemDef)
        {
            var result = new CMMNPlanItem(id, name, planItemDef);
            return result;
        }
    }
}