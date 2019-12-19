using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTaskProcessor : BaseCMMNTaskProcessor
    {
        public override CMMNPlanItemDefinitionTypes Type => CMMNPlanItemDefinitionTypes.Task;

        protected override Task Run(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            parameter.WorkflowInstance.MakeTransition(parameter.PlanItemInstance.Id, CMMNPlanItemTransitions.Complete);
            return Task.CompletedTask;
        }
    }
}
