using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTaskProcessor : BaseCMMNTaskProcessor
    {
        public override CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.Task;

        protected override Task Run(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Complete);
            return Task.CompletedTask;
        }
    }
}
