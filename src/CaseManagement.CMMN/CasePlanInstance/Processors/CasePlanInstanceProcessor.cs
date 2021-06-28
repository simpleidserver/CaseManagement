using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Common.Domains;
using MassTransit;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CasePlanInstanceProcessor : ICasePlanInstanceProcessor
    {
        private readonly ICMMNProcessorFactory _processorFactory;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IBusControl _busControl;

        public CasePlanInstanceProcessor(
            ICMMNProcessorFactory processorFactory, 
            ISubscriberRepository subscriberRepository,
            IBusControl busControl)
        {
            _processorFactory = processorFactory;
            _subscriberRepository = subscriberRepository;
            _busControl = busControl;
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
                if (casePlanInstance.StageContent.TakeStageState == TaskStageStates.Completed)
                {
                    casePlanInstance.MakeTransition(CMMNTransitions.Complete, false);
                    return;
                }

                if (casePlanInstance.StageContent.TakeStageState == TaskStageStates.Terminated)
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

            await Publish<CaseInstanceWorkerTaskAddedEvent>(casePlanInstance, cancellationToken);
            await Publish<CaseInstanceWorkerTaskRemovedEvent>(casePlanInstance, cancellationToken);
        }

        protected async Task Publish<TEvt>(CasePlanInstanceAggregate aggregate, CancellationToken cancellationToken) where TEvt : DomainEvent
        {
            var domainEvts = aggregate.DomainEvents.Where(e => e.GetType() == typeof(TEvt)).Cast<TEvt>();
            foreach(var domainEvt in domainEvts)
            {
                await _busControl.Publish(domainEvt, cancellationToken);
            }
        }
    }
}
