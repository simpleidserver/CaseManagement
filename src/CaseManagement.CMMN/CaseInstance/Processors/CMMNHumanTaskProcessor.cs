using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNHumanTaskProcessor : BaseCMMNTaskProcessor
    {
        public override CaseElementTypes Type => CaseElementTypes.HumanTask;

        private FormInstanceSubmittedListener _listener;

        protected override Task Run(ProcessorParameter parameter, CancellationToken token)
        {
            var humanTask = (parameter.CaseInstance.GetWorkflowElementDefinition(parameter.CaseElementInstance.Id, parameter.CaseDefinition) as PlanItemDefinition).PlanItemDefinitionHumanTask;
            parameter.CaseInstance.CreateFormInstance(parameter.CaseElementInstance.Id, humanTask.FormId, humanTask.PerformerRef);
            if (humanTask.IsBlocking)
            {
                _listener = CMMNFormInstanceSubmittedListener.Listen(parameter, token);
            }

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
