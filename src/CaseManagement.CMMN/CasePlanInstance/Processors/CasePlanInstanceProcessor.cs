using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CasePlanInstanceProcessor : ICasePlanInstanceProcessor
    {
        private readonly IProcessorFactory _processorFactory;
        private readonly ISubscriberRepository _subscriberRepository;

        public CasePlanInstanceProcessor(IProcessorFactory processorFactory, ISubscriberRepository subscriberRepository)
        {
            _processorFactory = processorFactory;
            _subscriberRepository = subscriberRepository;
        }

        public async Task Execute(CasePlanInstanceAggregate casePlanInstance, CancellationToken cancellationToken)
        {
            var terminateSub = await _subscriberRepository.TrySubscribe(casePlanInstance.AggregateId, CMMNConstants.ExternalTransitionNames.Terminate, cancellationToken);
            if (casePlanInstance.State == null)
            {
                casePlanInstance.MakeTransition(CMMNTransitions.Create);
            }

            if (casePlanInstance.State == CaseStates.Active)
            {
                var executionContext = new CMMNExecutionContext { Instance = casePlanInstance };
                foreach (var fileItem in casePlanInstance.FileItems)
                {
                    await _processorFactory.Execute(executionContext, fileItem  , cancellationToken);
                }

                await _processorFactory.Execute(executionContext, casePlanInstance.StageContent, cancellationToken);
                if (casePlanInstance.StageContent.State == TaskStageStates.Completed)
                {
                    casePlanInstance.MakeTransition(CMMNTransitions.Complete, false);
                    return;
                }

                if (casePlanInstance.StageContent.State == TaskStageStates.Terminated)
                {
                    casePlanInstance.MakeTransition(CMMNTransitions.Terminate, false);
                    return;
                }

                if (terminateSub.IsCaptured)
                {
                    await _subscriberRepository.TryReset(casePlanInstance.AggregateId, null, CMMNConstants.ExternalTransitionNames.Terminate, cancellationToken);
                    casePlanInstance.MakeTransition(CMMNTransitions.Terminate);
                }
            }
        }
    }
}
