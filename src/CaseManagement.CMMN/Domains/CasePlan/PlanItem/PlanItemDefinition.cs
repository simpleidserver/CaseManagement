namespace CaseManagement.CMMN.Domains
{
    public class PlanItemDefinition : CasePlanElement
    {
        public PlanItemDefinition(string id, string name) : base(id, name) { }

        public bool IsDiscretionary { get; set; }
        /// <summary>
        /// Reference to the corresponding PlanItemDefinition object. [1...1]
        /// </summary>
        public CMMNTask PlanItemDefinitionTask { get; set; }
        public HumanTask PlanItemDefinitionHumanTask { get; set; }
        public ProcessTask PlanItemDefinitionProcessTask { get; set; }
        public TimerEventListener PlanItemDefinitionTimerEventListener { get; set; }
        public Milestone PlanItemMilestone { get; set; }
        public StageDefinition Stage { get; set; }

        public static PlanItemDefinition New(string id, string name, StageDefinition stage)
        {
            var result = new PlanItemDefinition(id, name)
            {
                Stage = stage,
                Type = CaseElementTypes.Stage
            };
            return result;
        }

        public static PlanItemDefinition New(string id, string name, HumanTask humanTask)
        {
            var result = new PlanItemDefinition(id, name)
            {
                PlanItemDefinitionHumanTask = humanTask,
                Type = CaseElementTypes.HumanTask
            };
            return result;
        }

        public static PlanItemDefinition New(string id, string name, Milestone milestone)
        {
            var result = new PlanItemDefinition(id, name)
            {
                PlanItemMilestone = milestone,
                Type = CaseElementTypes.Milestone
            };
            return result;
        }

        public static PlanItemDefinition New(string id, string name, ProcessTask processTask)
        {
            var result = new PlanItemDefinition(id, name)
            {
                PlanItemDefinitionProcessTask = processTask,
                Type = CaseElementTypes.ProcessTask
            };
            return result;
        }

        public static PlanItemDefinition New(string id, string name, CMMNTask task)
        {
            var result = new PlanItemDefinition(id, name)
            {
                PlanItemDefinitionTask = task,
                Type = CaseElementTypes.Task
            };
            return result;
        }

        public static PlanItemDefinition New(string id, string name, TimerEventListener timer)
        {
            var result = new PlanItemDefinition(id, name)
            {
                PlanItemDefinitionTimerEventListener = timer,
                Type = CaseElementTypes.TimerEventListener
            };
            return result;
        }
    }
}