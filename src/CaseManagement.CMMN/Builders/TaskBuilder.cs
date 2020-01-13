using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class TaskBuilder : WorkflowElementBuilder
    {
        public TaskBuilder(PlanItemDefinition planItem) : base(planItem)
        {
        }

        public TaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }
    }
}
