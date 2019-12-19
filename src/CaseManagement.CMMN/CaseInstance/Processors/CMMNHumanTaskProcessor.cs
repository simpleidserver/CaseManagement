namespace CaseManagement.CMMN.CaseInstance.Processors
{
    /*
    public class CMMNHumanTaskProcessor : BaseCMMNTaskProcessor
    {
        public CMMNHumanTaskProcessor(IProcessorHelper processorHelper) : base(processorHelper)
        {
        }

        public override string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.HumanTask).ToLowerInvariant();

        protected override async Task Run(WorkflowHandlerContext context, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var humanTask = context.GetCMMNHumanTask();
            var formInstance = pf.GetFormInstance(context.CurrentElement.Id);
            if (formInstance == null && !string.IsNullOrWhiteSpace(humanTask.FormId))
            {
                pf.CreateForm(cmmnPlanItem.Id, humanTask.FormId, humanTask.PerformerRef);
            }

            else if (formInstance != null && formInstance.Status == FormInstanceStatus.Complete)
            {
                pf.CompletePlanItem(cmmnPlanItem);
                // await context.ExecuteNext(token);
                return;
            }

            if (humanTask.IsBlocking)
            {
                pf.BlockElement(cmmnPlanItem);
                return;
            }

            pf.CompletePlanItem(cmmnPlanItem);
            return;
        }
    }
    */
}
