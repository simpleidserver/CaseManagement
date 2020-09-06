using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CasePlanInstanceProcessor : ICasePlanInstanceProcessor
    {
        private readonly IProcessorFactory _processorFactory;

        public CasePlanInstanceProcessor(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }

        public async Task Execute(CasePlanInstanceAggregate casePlanInstance, CancellationToken cancellationToken)
        {
            if (casePlanInstance.State == null)
            {
                casePlanInstance.MakeTransition(CMMNTransitions.Create);
            }

            if (casePlanInstance.State == CaseStates.Active)
            {
                var executionContext = new ExecutionContext { CasePlanInstance = casePlanInstance };
                await _processorFactory.Execute(executionContext, casePlanInstance.Content, cancellationToken);
                if (casePlanInstance.Content.State == TaskStageStates.Completed)
                {
                    casePlanInstance.MakeTransition(CMMNTransitions.Complete);
                }
            }
        }
    }
}
