using CaseManagement.CMMN.CasePlanInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using static CaseManagement.CMMN.CasePlanInstance.Processors.Listeners.CasePlanElementInstanceTransitionListener;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNHumanTaskProcessor : BaseCMMNTaskProcessor
    {
        private readonly CMMNServerOptions _options;
        private bool _continueExecution;
        private EventListener _listener;

        public CMMNHumanTaskProcessor(IOptions<CMMNServerOptions> options, ICommitAggregateHelper commitAggregateHelper) : base(commitAggregateHelper)
        {
            _options = options.Value;
        }

        public override CaseElementTypes Type => CaseElementTypes.HumanTask;

        protected override async Task Run(ProcessorParameter parameter, CancellationToken token)
        {
            var humanTask = (parameter.CaseInstance.GetWorkflowElementDefinition(parameter.CaseElementInstance.Id, parameter.CaseDefinition) as PlanItemDefinition).PlanItemDefinitionHumanTask;
            _continueExecution = true;
            _listener = CasePlanElementInstanceTransitionListener.Listen(parameter, CMMNTransitions.Complete, () =>
            {
                _continueExecution = false;
            });
            if (!string.IsNullOrWhiteSpace(humanTask.FormId))
            {
                var formInstance = FormInstanceAggregate.New(humanTask.FormId, parameter.CaseInstance.Id, parameter.CaseElementInstance.Id, humanTask.PerformerRef);
                await CommitAggregateHelper.Commit(formInstance, FormInstanceAggregate.GetStreamName(formInstance.Id), CMMNConstants.QueueNames.FormInstances);
            }

            while (_continueExecution)
            {
                Thread.Sleep(_options.BlockThreadMS);
            }
        }

        protected override void Unsubscribe()
        {
            if (_listener != null)
            {
                _listener.Unsubscribe();
            }

            _continueExecution = false;
        }
    }
}
