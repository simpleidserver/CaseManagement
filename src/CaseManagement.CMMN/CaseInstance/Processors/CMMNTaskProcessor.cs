using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTaskProcessor : BaseCMMNTaskProcessor
    {
        public override CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.Task;

        protected override Task Run(ProcessorParameter parameter, CancellationToken token)
        {
            parameter.WorkflowInstance.MakeTransitionComplete(parameter.WorkflowElementInstance.Id);
            return Task.CompletedTask;
        }

        protected override void Unsubscribe() { }
    }
}
