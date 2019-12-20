using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNStageProcessor : ICMMNPlanItemProcessor
    {
        public CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.Stage;

        public Task Handle(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(() => HandleTask(parameter, cancellationTokenSource), token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private void HandleTask(PlanItemProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            CMMNCriterionListener.Listen(parameter);
            var isManuallyActivated = CMMNManualActivationListener.Listen(parameter);
            if (!isManuallyActivated)
            {
                parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Start);
            }

            var cmmnStageDefinition = parameter.WorkflowInstance.GetWorkflowElementDefinition(parameter.WorkflowElementInstance.Id, parameter.WorkflowDefinition) as CMMNStageDefinition;
            foreach(var elt in cmmnStageDefinition.Elements)
            {
                parameter.WorkflowInstance.CreateWorkflowElementInstance(elt);
            }
        }
    }
}
