using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNHumanTaskProcessor : BaseCMMNTaskProcessor
    {
        public override CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.HumanTask;

        private FormInstanceSubmittedListener _listener;

        protected override Task Run(ProcessorParameter parameter, CancellationToken token)
        {
            var humanTask = (parameter.WorkflowInstance.GetWorkflowElementDefinition(parameter.WorkflowElementInstance.Id, parameter.WorkflowDefinition) as CMMNPlanItemDefinition).PlanItemDefinitionHumanTask;
            parameter.WorkflowInstance.CreateFormInstance(parameter.WorkflowElementInstance.Id, humanTask.FormId, humanTask.PerformerRef);
            if (humanTask.IsBlocking)
            {
                _listener = CMMNFormInstanceSubmittedListener.Listen(parameter);
            }

            // parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Complete);
            return Task.CompletedTask;
        }

        protected override void Unsubscribe()
        {
            if (_listener != null)
            {
                _listener.Unsubscribe();
            }
        }
    }
}
