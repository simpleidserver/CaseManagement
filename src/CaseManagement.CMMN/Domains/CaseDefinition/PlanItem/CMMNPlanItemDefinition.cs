namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItemDefinition : CMMNWorkflowElementDefinition
    {
        public CMMNPlanItemDefinition(string id, string name) : base(id, name) { }

        /// <summary>
        /// Reference to the corresponding PlanItemDefinition object. [1...1]
        /// </summary>
        public CMMNTask PlanItemDefinitionTask { get; set; }
        public CMMNHumanTask PlanItemDefinitionHumanTask { get; set; }
        public CMMNProcessTask PlanItemDefinitionProcessTask { get; set; }
        public CMMNTimerEventListener PlanItemDefinitionTimerEventListener { get; set; }
        public CMMNMilestone PlanItemMilestone { get; set; }

        public static CMMNPlanItemDefinition New(string id, string name, CMMNHumanTask humanTask)
        {
            var result = new CMMNPlanItemDefinition(id, name)
            {
                PlanItemDefinitionHumanTask = humanTask,
                Type = CMMNWorkflowElementTypes.HumanTask
            };
            return result;
        }

        public static CMMNPlanItemDefinition New(string id, string name, CMMNMilestone milestone)
        {
            var result = new CMMNPlanItemDefinition(id, name)
            {
                PlanItemMilestone = milestone,
                Type = CMMNWorkflowElementTypes.Milestone
            };
            return result;
        }

        public static CMMNPlanItemDefinition New(string id, string name, CMMNProcessTask processTask)
        {
            var result = new CMMNPlanItemDefinition(id, name)
            {
                PlanItemDefinitionProcessTask = processTask,
                Type = CMMNWorkflowElementTypes.ProcessTask
            };
            return result;
        }

        public static CMMNPlanItemDefinition New(string id, string name, CMMNTask task)
        {
            var result = new CMMNPlanItemDefinition(id, name)
            {
                PlanItemDefinitionTask = task,
                Type = CMMNWorkflowElementTypes.Task
            };
            return result;
        }

        public static CMMNPlanItemDefinition New(string id, string name, CMMNTimerEventListener timer)
        {
            var result = new CMMNPlanItemDefinition(id, name)
            {
                PlanItemDefinitionTimerEventListener = timer,
                Type = CMMNWorkflowElementTypes.TimerEventListener
            };
            return result;
        }
    }
}